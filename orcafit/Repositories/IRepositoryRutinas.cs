using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Repositories
{
    public interface IRepositoryRutinas
    {
        Rutina GetRutina(int id);
        int InsertRutina
            (string nombre, string rutinatexto, string video, string imagen, string categoria);
        List<Rutina> GetRutinas();
        void DeleteRutina(int id);
    }
}
