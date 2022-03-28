using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    public class RutinaComenzada
    {
        public int IdRutinaComenzada { get; set; }
        public int IdUsuario { get; set; }
        public int IdRutina { get; set; }
    }
}
