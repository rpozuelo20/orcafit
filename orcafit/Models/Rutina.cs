using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    [Table("RUTINAS")]
    public class Rutina
    {
        [Key]
        [Column("RUTINA_NO")]
        public int IdRutina { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("RUTINA_TEXTO")]
        public string RutinaTexto { get; set; }
        [Column("VIDEO")]
        public string Video { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("CATEGORIA")]
        public string Categoria { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
