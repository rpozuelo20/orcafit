using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models.ViewModels
{
    public class ComentarioUsuarioViewModel
    {
        public int IdComentario { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime Fecha { get; set; }
        public string Username { get; set; }
        public string Imagen { get; set; }
    }
}
