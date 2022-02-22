using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    [Table("RUTINASCOMENZADAS")]
    public class RutinaComenzada
    {
        [Key]
        [Column("RUTINACOMENZADA_NO")]
        public int IdRutinaComenzada { get; set; }
        [Column("USER_NO")]
        public int IdUsuario { get; set; }
        [Column("RUTINA_NO")]
        public int IdRutina { get; set; }
    }
}
