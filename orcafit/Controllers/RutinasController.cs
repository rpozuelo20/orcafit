using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using orcafit.Models;
using orcafit.Models.ViewModels;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class RutinasController : Controller
    {
        #region INYECCION DE DEPENDENCIAS
        private ServiceRutinas service;
        public RutinasController(ServiceRutinas service)
        {
            this.service = service;
        }
        #endregion


        //  Vista de error:
        public IActionResult ErrorRutinas()
        {
            return View();
        }
        public IActionResult ErrorRutina()
        {
            return View();
        }
        //  Vista de rutinas:
        public async Task<IActionResult> Index()
        {
            ViewBag.Categorias = await this.service.GetCategoriasAsync();
            List<Rutina> rutinas = await this.service.GetRutinasAsync();
            if (rutinas != null)
            {
                Thread.Sleep(1000);
                return View(rutinas);
            }
            else
            {
                Thread.Sleep(1000);
                return RedirectToAction("ErrorRutinas");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(string nombre, string categoria)
        {
            ViewBag.Categorias = await this.service.GetCategoriasAsync();
            List<Rutina> rutinas = await this.service.GetRutinasAsync();
            if (rutinas != null)
            {
                if(nombre!=null && categoria == null)
                {
                    List<Rutina> buscador = new List<Rutina>();
                    foreach (Rutina rutina in rutinas)
                    {
                        if (rutina.Nombre.ToLower().Contains(nombre.ToLower()))
                        {
                            buscador.Add(rutina);
                        }
                    }
                    Thread.Sleep(1000);
                    return View(buscador);
                } else if(nombre==null && categoria != null)
                {
                    List<Rutina> buscador = new List<Rutina>();
                    foreach (Rutina rutina in rutinas)
                    {
                        if (rutina.Categoria.ToLower().Contains(categoria.ToLower()))
                        {
                            buscador.Add(rutina);
                        }
                    }
                    Thread.Sleep(1000);
                    return View(buscador);
                }
                else if(nombre!=null && categoria != null)
                {
                    List<Rutina> buscador = new List<Rutina>();
                    foreach (Rutina rutina in rutinas)
                    {
                        if (rutina.Nombre.ToLower().Contains(nombre.ToLower()) && rutina.Categoria.ToLower().Contains(categoria.ToLower()))
                        {
                            buscador.Add(rutina);
                        }
                    }
                    Thread.Sleep(1000);
                    return View(buscador);
                }
                else
                {
                    Thread.Sleep(1000);
                    return View(rutinas);
                }
            }
            else
            {
                Thread.Sleep(1000);
                return RedirectToAction("ErrorRutinas");
            }
        }
        //  Vista de rutina:
        [AuthorizeUsuarios]
        public async Task<IActionResult> Rutina(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Rutina rutina = await this.service.GetRutinaAsync(id, token);
            if (rutina != null)
            {
                List<ComentarioUsuarioViewModel> comentarios = await this.service.GetComentariosRutinaAsync(id, token);
                ViewBag.Comentarios = comentarios;
                ViewBag.ComentariosTotales = comentarios.Count();
                Thread.Sleep(1000);
                return View(rutina);
            }
            else
            {
                Thread.Sleep(1000);
                return RedirectToAction("ErrorRutina");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Rutina(int idrutina, string comentariotexto)
        {
            if (comentariotexto != null)
            {
                string token = HttpContext.User.FindFirst("TOKEN").Value;
                await this.service.InsertComentarioAsync(idrutina, comentariotexto, token);
                Thread.Sleep(1000);
                return RedirectToAction("Rutina", new { id = idrutina });
            }
            else
            {
                Thread.Sleep(1000);
                return RedirectToAction("Rutina", new { id = idrutina });
            }
        }
    }
}