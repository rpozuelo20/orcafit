using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Filters;
using orcafit.Helpers;
using orcafit.Models;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class ManageController : Controller
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private HelperTokenCallApi helperApi;
        private ServiceRutinas serviceRutinas;
        private ServiceUsuarios serviceUsuarios;
        private ServiceStorageBlobs serviceBlobs;
        public ManageController(HelperTokenCallApi helperApi, ServiceRutinas serviceRutinas, ServiceUsuarios serviceUsuarios, ServiceStorageBlobs serviceBlobs)
        {
            this.helperApi = helperApi;
            this.serviceRutinas = serviceRutinas;
            this.serviceUsuarios = serviceUsuarios;
            this.serviceBlobs = serviceBlobs;
        }
        //  Sentencias de inyeccion     ˄˄˄


        public IActionResult LogIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated==false)
            {
                return View();
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            username = username.ToLower();
            string token = await this.helperApi.GetTokenAsync(username, password);

            if (token == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas.";
                return View();
            } else
            {
                Usuario usuario = await this.helperApi.GetPerfilUsuarioAsync(token);
                ClaimsIdentity identity = new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Role));
                identity.AddClaim(new Claim("image", usuario.Imagen));
                identity.AddClaim(new Claim("iduser", usuario.IdUser.ToString()));
                identity.AddClaim(new Claim("fecha", usuario.Fecha.ToString().Substring(0, 10)));
                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult SignUp()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(string username, string password, IFormFile imagen)
        {
            username = username.ToLower();
            Usuario usuario = await this.serviceUsuarios.ExisteUsuarioAsync(username);
            if (usuario == null)
            {
                string blobName = username + "_" + imagen.FileName;
                using(Stream stream = imagen.OpenReadStream())
                {
                    await this.serviceBlobs.UploadBlobAsync("usuarioscontainer", blobName, stream);
                }
                BlobClass blob = await this.serviceBlobs.GetBlobAsync("usuarioscontainer", blobName);
                await this.serviceUsuarios.InsertUsuarioAsync(username, password, blob.Url);
                return RedirectToAction("LogIn");
            }
            else
            {
                ViewData["MENSAJE"] = "El usuario ya existe, inicie sesión.";
                return View();
            }
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");
            return RedirectToAction("Index", "Home");
        }
        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            ViewBag.ViewName = HttpContext.User.Identity.Name.ToString().ToLower();
            ViewBag.UserRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            ViewBag.UserName = HttpContext.User.Identity.Name.ToString();
            ViewBag.UserImage = HttpContext.User.FindFirst("image").Value.ToString();
            ViewBag.UserFecha = HttpContext.User.FindFirst("fecha").Value.ToString();
            return View();
        }
    }
}