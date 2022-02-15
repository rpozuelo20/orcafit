using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orcafit.Data;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public class RepositoryRutinas : IRepositoryRutinas
    {
        //  Sentencias comunes en los repositories   ⌄⌄⌄
        private orcafitContext context;
        public RepositoryRutinas(orcafitContext context)
        {
            this.context = context;
        }
        //  Sentencias comunes en los repositories   ˄˄˄


        private int GetMaxIdRutina()
        {
            if (this.context.Rutinas.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Rutinas.Max(z => z.IdRutina) + 1;
            }
        }

        public List<Categoria> GetCategorias() 
        {
            var consulta = from datos in this.context.Categorias
                           select datos;
            return consulta.ToList();
        }

        public Rutina GetRutina(int id)
        {
            return this.context.Rutinas.SingleOrDefault(x => x.IdRutina == id);
        }
        public List<Rutina> GetRutinas()
        {
            var consulta = from datos in this.context.Rutinas
                           select datos;
            return consulta.ToList();
        }
        public int InsertRutina
            (string nombre, string rutinatexto, string video, string imagen, string categoria)
        {
            int idrutina = this.GetMaxIdRutina();
            Rutina rutina = new Rutina();
            rutina.IdRutina = idrutina;
            rutina.Nombre = nombre;
            rutina.RutinaTexto = rutinatexto;
            rutina.Video = idrutina + "_" + video;
            rutina.Imagen = idrutina + "_" + imagen;
            rutina.Categoria = categoria;
            rutina.Fecha = DateTime.Now;

            this.context.Rutinas.Add(rutina);
            this.context.SaveChanges();

            return idrutina;
        }
        public void DeleteRutina(int id)
        {
            this.context.Rutinas.Remove(GetRutina(id));
            this.context.SaveChanges();
        }
    }
}
