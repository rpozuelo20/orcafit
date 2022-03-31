using Microsoft.AspNetCore.Mvc;
using orcafit.Models;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class UsuariosController : Controller
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private ServiceUsuarios service;
        public UsuariosController(ServiceUsuarios service)
        {
            this.service = service;
        }
        //  Sentencias de inyeccion     ˄˄˄

        public async Task<IActionResult> Index()
        {
            List<Usuario> usuarios = await this.service.GetUsuarios();
            return View(usuarios);
        }
    }
}
