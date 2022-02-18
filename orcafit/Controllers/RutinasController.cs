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
        [HttpPost]
        public IActionResult Index(string nombre, string categoria)
        {
            List<Rutina> rutinas = this.repo.GetRutinaNombre(nombre, categoria);
            List<Categoria> categorias = this.repo.GetCategorias();
            ViewBag.Categorias = categorias;
            return View(rutinas);
        }

        [AuthorizeUsuarios]
        public IActionResult Rutina(int id)
        {
            List<Comentario> comentarios = this.repo.GetComentarios(id);
            Rutina rutina = this.repo.GetRutina(id);
            if (rutina != null)
            {
                ViewBag.Comentarios = comentarios;
                return View(rutina);
            }
            else
            {
                return RedirectToAction("ErrorRutina", "Rutinas");
            }
        }
        [HttpPost]
        public IActionResult Rutina(int idrutina, string comentariotexto)
        {
            int iduser = int.Parse(HttpContext.User.FindFirst("iduser").Value);
            string username = HttpContext.User.Identity.Name.ToString().ToLower();
            username = char.ToUpper(username[0]) + username.Substring(1);
            string userimage = HttpContext.User.FindFirst("image").Value.ToString();

            this.repo.InsertComentario(idrutina, iduser, username, comentariotexto, userimage);
            return RedirectToAction("Rutina");
        }
    }
}
