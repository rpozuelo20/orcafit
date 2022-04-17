using Newtonsoft.Json;
using orcafit.Helpers;
using orcafit.Models;
using orcafit.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace orcafit.Services
{
    public class ServiceRutinas
    {
        #region INYECCION DE DEPENDENCIAS
        private HelperTokenCallApi helperApi;
        private Uri UriApi;
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceRutinas(HelperTokenCallApi helperApi, string url)
        {
            this.helperApi = helperApi;
            this.UriApi = new Uri(url);
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        #endregion


        //  Metodos para las rutinas.
        public async Task<List<Rutina>> GetRutinasAsync()
        {
            string request = "/api/rutinas";
            List<Rutina> rutinas = await this.helperApi.CallApiAsync<List<Rutina>>(request);
            return rutinas;
        }
        public async Task<Rutina> GetRutinaAsync(int idrutina, string token)
        {
            string request = "/api/rutinas/" + idrutina;
            Rutina rutina = await this.helperApi.CallApiAsync<Rutina>(request, token);
            return rutina;
        }
        public async Task InsertRutinaAsync(string nombre, string rutinatexto, string video, string imagen, string categoria, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/rutinas";
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Rutina rutina = new Rutina();
                rutina.Nombre = nombre;
                rutina.RutinaTexto = rutinatexto;
                rutina.Video = video;
                rutina.Imagen = imagen;
                rutina.Categoria = categoria;
                string json = JsonConvert.SerializeObject(rutina);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
        //  Metodos para las categorias.
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "/api/rutinas/categorias";
            List<Categoria> categorias = await this.helperApi.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }
        //  Metodos para los comentarios.
        public async Task<List<ComentarioUsuarioViewModel>> GetComentariosRutinaAsync(int idrutina, string token)
        {
            string request = "/api/rutinas/comentarios/"+idrutina;
            List<ComentarioUsuarioViewModel> comentarios = await this.helperApi.CallApiAsync<List<ComentarioUsuarioViewModel>>(request, token);
            return comentarios;
        }
        public async Task InsertComentarioAsync(int idrutina, string comentariotexto, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/rutinas/comentarios/" + idrutina;
                client.BaseAddress = this.UriApi;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Comentario comentario = new Comentario();
                comentario.ComentarioTexto = comentariotexto;
                string json = JsonConvert.SerializeObject(comentario);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
    }
}
