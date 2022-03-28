using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public int IdRutina { get; set; }
        public int IdUser { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
