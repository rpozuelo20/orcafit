using Newtonsoft.Json;
using orcafit.Helpers;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace orcafit.Services
{
    public class ServiceUsuarios
    {
        #region INYECCION DE DEPENDENCIAS
        private HelperTokenCallApi helperApi;
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceUsuarios(HelperTokenCallApi helperApi, string url)
        {
            this.helperApi = helperApi;
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        #endregion


        //  Metodos para los usuarios.
        public async Task<List<Usuario>> GetUsuarios()
        {
            string request = "/api/usuarios";
            List<Usuario> usuarios = await this.helperApi.CallApiAsync<List<Usuario>>(request);
            return usuarios;
        }
        public async Task<Usuario> ExisteUsuarioAsync(string username)
        {
            string request = "/api/usuarios/" + username;
            Usuario usuario = await this.helperApi.CallApiAsync<Usuario>(request);
            return usuario;
        }
        public async Task InsertUsuarioAsync(string username, string password, string image)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/usuarios";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Usuario usuario = new Usuario();
                usuario.Username = username;
                usuario.Password = password;
                usuario.Imagen = image;
                string json = JsonConvert.SerializeObject(usuario);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
    }
}
