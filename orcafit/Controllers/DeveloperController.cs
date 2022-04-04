using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using orcafit.Models;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    [AuthorizeAdmin]
    public class DeveloperController : Controller
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private ServiceRutinas service;
        private ServiceStorageBlobs serviceBlobs;
        public DeveloperController(ServiceRutinas service, ServiceStorageBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
        }
        //  Sentencias de inyeccion     ˄˄˄

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Rutinas()
        {
            List<Rutina> rutinas = await this.service.GetRutinasAsync();
            return View(rutinas);
        }
        public async Task<IActionResult> CreateRutina()
        {
            ViewBag.Categorias = await this.service.GetCategoriasAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRutina(string nombre, string rutinatexto, IFormFile video, IFormFile imagen, string categoria)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            string blobimageName = DateTime.UtcNow.ToString() + imagen.FileName;
            string blobvideoName = DateTime.UtcNow.ToString() + video.FileName;
            using(Stream streamvideo = video.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync("rutinascontainer", blobvideoName, streamvideo);
            }
            using (Stream streamimage = imagen.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync("rutinascontainer", blobimageName, streamimage);
            }
            BlobClass blobVideo = await this.serviceBlobs.GetBlobAsync("rutinascontainer", blobvideoName);
            BlobClass blobImage = await this.serviceBlobs.GetBlobAsync("rutinascontainer", blobimageName);
            await this.service.InsertRutinaAsync(nombre, rutinatexto, blobVideo.Url, blobImage.Url, categoria, token);
            return RedirectToAction("Rutinas", "Developer");
        }
        public IActionResult DeleteRutina()
        {
            return View();
        }
    }
}
