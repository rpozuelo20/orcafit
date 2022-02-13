using orcafit.Data;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        //  Sentencias comunes en los repositories   ⌄⌄⌄
        private orcafitContext context;
        public RepositoryUsuarios(orcafitContext context)
        {
            this.context = context;
        }
        //  Sentencias comunes en los repositories   ˄˄˄


        private int GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUser) + 1;
            }
        }

        public Usuario ExisteUsername(string username)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Username == username
                           select datos;
            return consulta.SingleOrDefault();
        }
        public Usuario ExisteUsuario(string username, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Username == username
                           && datos.Password == password
                           select datos;
            return consulta.SingleOrDefault();
        }
        public int InsertUsuario(string username, string password)
        {
            int idusuario = this.GetMaxIdUsuario();
            Usuario usuario = new Usuario();
            usuario.IdUser = idusuario;
            usuario.Username = username;
            usuario.Password = password;
            usuario.Role = "user";

            this.context.Usuarios.Add(usuario);
            this.context.SaveChanges();

            return idusuario;
        }
    }
}
