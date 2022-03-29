﻿using Newtonsoft.Json;
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
        //  Sentencias comunes en los services   ⌄⌄⌄
        private HelperTokenCallApi helperApi;
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceUsuarios(HelperTokenCallApi helperApi, string url)
        {
            this.helperApi = helperApi;
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        //  Sentencias comunes en los services   ˄˄˄

        
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
        //  El metodo existeusuariousername que accede a la ruta api/usuarios/{id} hay que cambiarlo en el api a no authorizado o protegido, debe de ser accedido.
        public async Task<Usuario> ExisteUsuarioUsername(string username)
        {
            string request = "/api/usuarios/" + username;
            Usuario usuario = await this.helperApi.CallApiAsync<Usuario>(request);
            return usuario;
        }
    }
}
