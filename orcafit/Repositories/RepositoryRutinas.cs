using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using orcafit.Data;
using orcafit.Models;
using orcafit.Models.ViewModels;
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

        public int InsertComentario(int idrutina, int iduser, string comentariotexto)
        {
            int idcomentario = this.GetMaxIdComentario();
            Comentario comentario = new Comentario();
            comentario.IdComentario = idcomentario;
            comentario.IdRutina = idrutina;
            comentario.IdUser = iduser;
            comentario.ComentarioTexto = comentariotexto;
            comentario.Fecha = DateTime.Now;

            this.context.Comentarios.Add(comentario);
            this.context.SaveChanges();

            return idcomentario;
        }
        public List<ComentarioUsuarioViewModel> GetComentarios(int idrutina)
        {
            var consulta = from comentario in this.context.Comentarios
                           join usuario in this.context.Usuarios
                           on comentario.IdUser equals usuario.IdUser
                           where comentario.IdRutina == idrutina
                           orderby comentario.IdComentario descending
                           select new ComentarioUsuarioViewModel
                           {
                               IdComentario = comentario.IdComentario,
                               ComentarioTexto = comentario.ComentarioTexto,
                               Fecha = comentario.Fecha,
                               Username = usuario.Username,
                               Imagen = usuario.Imagen
                           };

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

        public int InsertRutinaComenzada(int idrutina, int iduser)
        {
            int idrutinacomenzada = int.Parse(iduser.ToString() + idrutina.ToString());
            RutinaComenzada rutinacomenzada = new RutinaComenzada();
            rutinacomenzada.IdRutinaComenzada = idrutinacomenzada;
            rutinacomenzada.IdRutina = idrutina;
            rutinacomenzada.IdUsuario = iduser;

            this.context.RutinasComenzadas.Add(rutinacomenzada);
            this.context.SaveChanges();

            return idrutinacomenzada;
        }
        public List<RutinaComenzada> GetRutinasComenzadas(int iduser)
        {
            var consulta = from datos in this.context.RutinasComenzadas
                           where datos.IdUsuario == iduser
                           select datos;
            return consulta.ToList();
        }
        public void LimpiarRutinasComenzadas(int iduser)
        {
            List<RutinaComenzada> rutinascomenzadas = GetRutinasComenzadas(iduser);
            foreach(RutinaComenzada rutinacomenzada in rutinascomenzadas)
            {
                this.context.RutinasComenzadas.Remove(rutinacomenzada);
            }
            this.context.SaveChanges();
        }
    }
}
