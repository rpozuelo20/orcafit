using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using orcafit.Models;
using orcafit.Models.ViewModels;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class RutinasController : Controller
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private ServiceRutinas service;
        public RutinasController(ServiceRutinas service)
        {
            this.service = service;
        }
        //  Sentencias de inyeccion     ˄˄˄


        public async Task<IActionResult> Index()
        {
            ViewBag.Categorias = await this.service.GetCategoriasAsync();
            List<Rutina> rutinas = await this.service.GetRutinasAsync();
            return View(rutinas);
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> Rutina(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<ComentarioUsuarioViewModel> comentarios = await this.service.GetComentariosRutina(id, token);
            ViewBag.Comentarios = comentarios;
            ViewBag.ComentariosTotales = comentarios.Count();
            Rutina rutina = await this.service.GetRutinaAsync(id, token);
            return View(rutina);
        }
    }
}