using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    public class Rutina
    {
        public int IdRutina { get; set; }
        public string Nombre { get; set; }
        public string RutinaTexto { get; set; }
        public string Video { get; set; }
        public string Imagen { get; set; }
        public string Categoria { get; set; }
        public DateTime Fecha { get; set; }
    }
}
