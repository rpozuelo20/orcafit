using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public interface IRepositoryUsuarios
    {
        int InsertUsuario(string username, string password);
        Usuario ExisteUsuario(string username, string password);
        Usuario ExisteUsername(string username);
    }
}
