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


        //  Metodo para recuperar todas las rutinas:
        public async Task<List<Rutina>> GetRutinasAsync()
        {
            string request = "/api/rutinas";
            List<Rutina> rutinas = await this.helperApi.CallApiAsync<List<Rutina>>(request);
            return rutinas;
        }
        //  Metodo para recuperar una rutina:
        public async Task<Rutina> GetRutinaAsync(int idrutina, string token)
        {
            string request = "/api/rutinas/" + idrutina;
            Rutina rutina = await this.helperApi.CallApiAsync<Rutina>(request, token);
            return rutina;
        }
        //  Metodo para recuperar las categorias:
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            string request = "/api/rutinas/categorias";
            List<Categoria> categorias = await this.helperApi.CallApiAsync<List<Categoria>>(request);
            return categorias;
        }
        //  Metodo para recuperar los comentarios:
        public async Task<List<ComentarioUsuarioViewModel>> GetComentariosRutina(int idrutina, string token)
        {
            string request = "/api/rutinas/comentarios/"+idrutina;
            List<ComentarioUsuarioViewModel> comentarios = await this.helperApi.CallApiAsync<List<ComentarioUsuarioViewModel>>(request, token);
            return comentarios;
        }

        public async Task<List<Rutina>> GetRutinasComenzadasAsync(string token)
        {
            string request = "/api/rutinas/rutinascomenzadas";
            List<RutinaComenzada> rutinasComenzadas = await this.helperApi.CallApiAsync<List<RutinaComenzada>>(request, token);
            List<int> idRutinasComenzadas = new List<int>();
            List<Rutina> rutinas = await this.GetRutinasAsync();
            List<Rutina> misRutinas = new List<Rutina>();
            foreach(RutinaComenzada item in rutinasComenzadas)
            {
                idRutinasComenzadas.Add(item.IdRutina);
            }
            foreach(Rutina rutina in rutinas)
            {
                if (idRutinasComenzadas.Contains(rutina.IdRutina))
                {
                    misRutinas.Add(rutina);
                }
            }
            return rutinas;
        }
    }
}
