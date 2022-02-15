using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using orcafit.Models;
using orcafit.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class ManageController : Controller
    {
        //  Sentencias comunes en los controllers   ⌄⌄⌄
        private IRepositoryUsuarios repo;
        public ManageController(IRepositoryUsuarios repo)
        {
            this.repo = repo;
        }
        //  Sentencias comunes en los controllers   ˄˄˄


        public IActionResult ErrorAcceso()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            Usuario usuario = this.repo.ExisteUsuario(username, password);
            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName = new Claim(ClaimTypes.Name, usuario.Username);
                Claim claimRole = new Claim(ClaimTypes.Role, usuario.Role);

                identity.AddClaim(claimRole);
                identity.AddClaim(claimName);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Wrong username or password.";
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string username, string password)
        {
            Usuario usuario = this.repo.ExisteUsername(username);
            if (usuario == null)
            {
                this.repo.InsertUsuario(username, password);
                return RedirectToAction("LogIn", "Manage");
            } 
            else
            {
                ViewData["MENSAJE"] = "The user already exist.";
            }
            return View();
        }
    }
}
