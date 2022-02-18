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
        private int GetMaxIdComentario()
        {
            if (this.context.Comentarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Comentarios.Max(z => z.IdComentario) + 1;
            }
        }

        public int InsertComentario(int idrutina, int iduser, string username, string comentariotexto, string userimage)
        {
            int idcomentario = this.GetMaxIdComentario();
            Comentario comentario = new Comentario();
            comentario.IdComentario = idcomentario;
            comentario.IdRutina = idrutina;
            comentario.Username = username;
            comentario.IdUser = iduser;
            comentario.ComentarioTexto = comentariotexto;
            comentario.Fecha = DateTime.Now;
            comentario.UserImage = userimage;

            this.context.Comentarios.Add(comentario);
            this.context.SaveChanges();

            return idcomentario;
        }
        public List<Comentario> GetComentarios(int id)
        {
            var consulta = from datos in this.context.Comentarios
                           where datos.IdRutina == id
                           orderby datos.IdComentario descending
                           select datos;
            return consulta.ToList();
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
        public List<Rutina> GetRutinaNombre(string nombre, string categoria)
        {
            if(nombre != null && categoria == null)
            {
                var consulta = from datos in this.context.Rutinas
                               where datos.Nombre.Contains(nombre)
                               select datos;
                return consulta.ToList();
            }
            else if(nombre == null && categoria != null)
            {
                var consulta = from datos in this.context.Rutinas
                               where datos.Categoria.ToLower() == categoria.ToLower()
                               select datos;
                return consulta.ToList();
            }
            else if(nombre != null && categoria != null)
            {
                var consulta = from datos in this.context.Rutinas
                               where datos.Categoria.ToLower() == categoria.ToLower()
                               && datos.Nombre.Contains(nombre)
                               select datos;
                return consulta.ToList();
            } else
            {
                var consulta = from datos in this.context.Rutinas
                               select datos;
                return consulta.ToList();
            }
        }
    }
}
