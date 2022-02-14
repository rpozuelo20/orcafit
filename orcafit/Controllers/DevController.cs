using betari_app.Helpers;
using betari_app.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using orcafit.Models;
using orcafit.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    [AuthorizeAdmin]
    public class DevController : Controller
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryRutinas repo;
        private HelperUploadFiles helper;
        public DevController(IRepositoryRutinas repo, HelperUploadFiles helper)
        {
            this.repo = repo;
            this.helper = helper;
        }
        //  Sentencias comunes en los controllers   ˄˄˄


        public IActionResult VistaRutinas()
        {
            List<Rutina> rutinas = this.repo.GetRutinas();
            return View(rutinas);
        }
        public IActionResult DeleteRutina(int id, string imagen, string video)
        {
            this.repo.DeleteRutina(id);
            
            System.IO.File.Delete("wwwroot/uploadFiles/images/"+imagen);
            System.IO.File.Delete("wwwroot/uploadFiles/videos/"+video);
            return RedirectToAction("VistaRutinas");
        }
        public IActionResult CreateRutina()
        {
            List<Categoria> categorias = this.repo.GetCategorias();
            ViewBag.Categorias = categorias;
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4294967295)]
        public async Task<IActionResult> CreateRutina
            (string nombre, string rutinatexto, IFormFile video, IFormFile imagen, string categoria)
        {
            string fileNameImage = imagen.FileName;
            string fileNameVideo = video.FileName;

            int idrutina = this.repo.InsertRutina(nombre, rutinatexto, fileNameVideo, fileNameImage, categoria);
            fileNameImage = idrutina + "_" + fileNameImage;
            fileNameVideo = idrutina + "_" + fileNameVideo;

            await this.helper.UploadFileAsync(imagen, Folders.Images, fileNameImage);
            await this.helper.UploadFileAsync(video, Folders.Videos, fileNameVideo);
            return RedirectToAction("VistaRutinas");
        }
    }
}
