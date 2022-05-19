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
using System.Threading;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class ManageController : Controller
    {
        #region INYECCION DE DEPENDENCIAS
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
        #endregion


        //  Vista de error:
        public IActionResult ErrorAcceso()
        {
            return View();
        }
        //  Vista de login:
        public IActionResult LogIn()
        {
            //  Si el usuario no esta autenticado nos podra devolver la vista, en caso contrario nos devolvera Home.
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
            //  Si el usuario y contraseña son distintos de null seguimos, si no nos dara un error y devolvera la vista.
            if(username != null && password != null)
            {
                //  Guardamos el Token mediante los datos enviador, si el null nos da error y devuelve la vista, si no comienza a realizar el login almacenando los datos en identity.
                username = username.ToLower();
                string token = await this.helperApi.GetTokenAsync(username, password);
                if (token == null)
                {
                    ViewData["MENSAJE"] = "Credenciales incorrectas.";
                    Thread.Sleep(1000);
                    return View();
                }
                else
                {
                    Usuario usuario = await this.helperApi.GetPerfilUsuarioAsync(token);
                    ClaimsIdentity identity = new ClaimsIdentity
                        (CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Username));
                    identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Role));
                    identity.AddClaim(new Claim("image", usuario.Imagen));
                    identity.AddClaim(new Claim("iduser", usuario.IdUser.ToString()));
                    identity.AddClaim(new Claim("fecha", usuario.Fecha.ToShortDateString()));
                    identity.AddClaim(new Claim("TOKEN", token));
                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                    Thread.Sleep(1000);
                    return RedirectToAction("Index", "Home");
                }
            } else
            {
                ViewData["MENSAJE"] = "Introduzca unas credenciales.";
                Thread.Sleep(1000);
                return View();
            }
        }
        //  Vista de verificacion:
        public IActionResult VerificarUsuarioStart()
        {
            return View();
        }
        public IActionResult VerificarUsuarioEnd()
        {
            return View();
        }
        //  Vista de signup:
        public IActionResult SignUp()
        {
            //  Si el usuario no esta autenticado nos podra devolver la vista, en caso contrario nos devolvera Home.
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
            //  Si el usuario, contraseña e imagen son distintos de null podemos, en caso contrario nos devuelve un error y la vista.
            if(username != null && password != null && imagen != null)
            {
                //  Convertimos el usuario a ToLower y vemos si existe en nuestra BBDD, de ser el caso, nos devuelve un error y la vista.
                username = username.ToLower();
                Usuario usuario = await this.serviceUsuarios.ExisteUsuarioAsync(username);
                if (usuario == null)
                {
                    var imageSize = imagen.Length;
                    if (imageSize < 100000) 
                    {
                        //  Si el usuario no existe entonces podemos empezar almacenandolo en nuestra BBDD, redirigimos hacia la vista login.
                        string blobName = username + "_" + imagen.FileName;
                        using (Stream stream = imagen.OpenReadStream())
                        {
                            await this.serviceBlobs.UploadBlobAsync("usuarioscontainer", blobName, stream);
                        }
                        BlobClass blob = await this.serviceBlobs.GetBlobAsync("usuarioscontainer", blobName);
                        await this.serviceUsuarios.InsertUsuarioAsync(username, password, blob.Url);
                        Thread.Sleep(1000);
                        return RedirectToAction("LogIn");
                    }
                    else
                    {
                        ViewData["MENSAJE"] = "La imagen es muy grande, máximo 100KB.";
                        Thread.Sleep(1000);
                        return View();
                    }
                }
                else
                {
                    ViewData["MENSAJE"] = "El usuario ya existe, inicie sesión.";
                    Thread.Sleep(1000);
                    return View();
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Introduzca unas credenciales.";
                Thread.Sleep(1000);
                return View();
            }
        }
        //  Vista de logout:
        [AuthorizeUsuarios]
        public async Task<IActionResult> LogOut()
        {
            //  Cerramos Sesion, borramos el Token y redirigimos hacia Home.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");
            return RedirectToAction("Index", "Home");
        }
        //  Vista de perfil:
        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            //  Almacenamos los valores contenidos en el Context, contienen los datos del usuario iniciado en sesion.
            ViewBag.ViewName = HttpContext.User.Identity.Name.ToString().ToLower();
            ViewBag.UserRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            ViewBag.UserName = HttpContext.User.Identity.Name.ToString();
            ViewBag.UserImage = HttpContext.User.FindFirst("image").Value.ToString();
            ViewBag.UserFecha = HttpContext.User.FindFirst("fecha").Value.ToString();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PerfilUsuario(IFormFile imagen)
        {
            //  Recupero el token y usuario antiguos con los que he iniciado la sesion.
            string oldToken = HttpContext.User.FindFirst("TOKEN").Value;
            Usuario oldUsuario = await this.helperApi.GetPerfilUsuarioAsync(oldToken);
            //  Recupero el tamaño de KB que tiene la imagen.
            var imageSize = imagen.Length;
            //  Si la imagen es menor a 100KB entonces puede proseguir.
            if (imageSize < 100000)
            {
                //  Parte de eliminacion del antiguo blob y añado el nuevo.
                BlobClass oldBlob = await this.serviceBlobs.GetBlobWithUrlAsync("usuarioscontainer", oldUsuario.Imagen);
                await this.serviceBlobs.DeleteBlobAsync("usuarioscontainer", oldBlob.Nombre);
                string blobName = oldUsuario.Username + "_" + imagen.FileName;
                using (Stream stream = imagen.OpenReadStream())
                {
                    await this.serviceBlobs.UploadBlobAsync("usuarioscontainer", blobName, stream);
                }
                BlobClass blob = await this.serviceBlobs.GetBlobAsync("usuarioscontainer", blobName);
                //  Actualizo la imagen de usuario con la nueva url y el token antiguo.
                await this.serviceUsuarios.UpdateImagenUsuarioAsync(blob.Url, oldToken);
                //  Recupero el token y usuario nuevo con el que he iniciado la sesion.
                string newToken = await this.helperApi.GetTokenAsync(oldUsuario.Username, oldUsuario.Password);
                Usuario newUsuario = await this.helperApi.GetPerfilUsuarioAsync(newToken);
                //  Cierro la sesion y remuevo las cookies y el token.
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Remove("TOKEN");
                //  Añado nuevas cookies y token.
                ClaimsIdentity identity = new ClaimsIdentity
                        (CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, newUsuario.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, newUsuario.Role));
                identity.AddClaim(new Claim("image", newUsuario.Imagen));
                identity.AddClaim(new Claim("iduser", newUsuario.IdUser.ToString()));
                identity.AddClaim(new Claim("fecha", newUsuario.Fecha.ToShortDateString()));
                identity.AddClaim(new Claim("TOKEN", newToken));
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });
                //  Esperamos 1 segundo y reenviamos la vista al usuario.
                TempData["MENSAJE"] = "Perfil actualizado con éxito.";
                Thread.Sleep(1000);
                return RedirectToAction("PerfilUsuario");
            }
            else
            {
                TempData["MENSAJE"] = "La imagen es muy grande, máximo 100KB.";
                Thread.Sleep(1000);
                return RedirectToAction("PerfilUsuario");
            }
        }
    }
}