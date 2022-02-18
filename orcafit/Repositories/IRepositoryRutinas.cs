using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public interface IRepositoryRutinas
    {
        int InsertComentario(int idrutina, int iduser, string username, string comentariotexto);
        List<Comentario> GetComentarios(int id);
        List<Categoria> GetCategorias();
        Rutina GetRutina(int id);
        List<Rutina> GetRutinas();
        int InsertRutina
            (string nombre, string rutinatexto, string video, string imagen, string categoria);
        void DeleteRutina(int id);
        List<Rutina> GetRutinaNombre(string nombre, string categoria);
    }
}
