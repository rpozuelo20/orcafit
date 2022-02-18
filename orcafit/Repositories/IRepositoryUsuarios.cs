using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public interface IRepositoryUsuarios
    {
        Usuario GetUsuario(string username);
        void DeleteUsuario(string username);
        int InsertUsuario(string username, string password, string imagen);
        Usuario ExisteUsuario(string username, string password);
        Usuario ExisteUsername(string username);
    }
}
