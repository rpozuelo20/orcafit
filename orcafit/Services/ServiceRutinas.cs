using orcafit.Helpers;
using orcafit.Models;
using orcafit.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace orcafit.Services
{
    public class ServiceRutinas
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private HelperTokenCallApi helperApi;
        public ServiceRutinas(HelperTokenCallApi helperApi)
        {
            this.helperApi = helperApi;
        }
        //  Sentencias de inyeccion     ˄˄˄


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

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "/api/rutinas/categorias";
            List<Categoria> categorias = await this.helperApi.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }

        public async Task<List<ComentarioUsuarioViewModel>> GetComentariosRutina(int idrutina, string token)
        {
            string request = "/api/rutinas/comentarios/"+idrutina;
            List<ComentarioUsuarioViewModel> comentarios = await this.helperApi.CallApiAsync<List<ComentarioUsuarioViewModel>>(request, token);
            return comentarios;
        }
    }
}
