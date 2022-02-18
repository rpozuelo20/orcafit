using betari_app.Helpers;
using betari_app.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
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
        private HelperUploadFiles helper;
        public ManageController(IRepositoryUsuarios repo, HelperUploadFiles helper)
        {
            this.repo = repo;
            this.helper = helper;
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
                Claim claimImage = new Claim("image", usuario.Imagen);
                Claim claimId = new Claim("iduser", usuario.IdUser.ToString());
                Claim claimFecha = new Claim("fecha", usuario.Fecha.ToString().Substring(0, 10));

                identity.AddClaim(claimRole);
                identity.AddClaim(claimName);
                identity.AddClaim(claimImage);
                identity.AddClaim(claimId);
                identity.AddClaim(claimFecha);

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
        public async Task<IActionResult> SignUp(string username, string password, IFormFile imagen)
        {
            string fileNameImage = imagen.FileName;
            Usuario usuario = this.repo.ExisteUsername(username);
            if (usuario == null)
            {
                this.repo.InsertUsuario(username, password, fileNameImage);
                fileNameImage = username + "_" + fileNameImage;
                await this.helper.UploadFileAsync(imagen, Folders.Users, fileNameImage);
                return RedirectToAction("LogIn", "Manage");
            } 
            else
            {
                ViewData["MENSAJE"] = "The user already exist.";
            }
            return View();
        }
        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }
        public async Task<IActionResult> DeleteUsuario(string username)
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            this.repo.DeleteUsuario(username);
            return RedirectToAction("Index", "Home");
        }
    }
}
