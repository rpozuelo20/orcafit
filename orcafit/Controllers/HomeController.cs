using Microsoft.AspNetCore.Mvc;
using orcafit.Models;
using orcafit.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class HomeController : Controller
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryUsuarios repo;
        public HomeController(IRepositoryUsuarios repo)
        {
            this.repo = repo;
        }
        //  Sentencias comunes en los controllers   ˄˄˄

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string correo)
        {
            Email email = this.repo.AddEmail(correo);
            return View();
        }
    }
}
