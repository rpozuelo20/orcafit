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
    public class RutinasController : Controller
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryRutinas repo;
        public RutinasController(IRepositoryRutinas repo)
        {
            this.repo = repo;
        }
        //  Sentencias comunes en los controllers   ˄˄˄


        [AuthorizeUsuarios]
        public IActionResult ErrorRutina()
        {
            return View();
        }

        public IActionResult Index()
        {
            List<Rutina> rutinas = this.repo.GetRutinas();
            List<Categoria> categorias = this.repo.GetCategorias();
            ViewBag.Categorias = categorias;
            return View(rutinas);
        }

        [AuthorizeUsuarios]
        public IActionResult Rutina(int id)
        {
            List<Rutina> rutinas = this.repo.GetRutinas();
            Rutina rutina = this.repo.GetRutina(id);
            if (rutina != null)
            {
                ViewBag.Rutinas = rutinas;
                return View(rutina);
            }
            else
            {
                return RedirectToAction("ErrorRutina", "Rutinas");
            }
        }
    }
}
