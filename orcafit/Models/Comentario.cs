using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    [Table("COMENTARIOS")]
    public class Comentario
    {
        [Key]
        [Column("COMENTARIO_NO")]
        public int IdComentario { get; set; }
        [Column("RUTINA_NO")]
        public int IdRutina { get; set; }
        [Column("USERNAME")]
        public string Username { get; set; }
        [Column("USER_NO")]
        public int IdUser { get; set; }
        [Column("COMENTARIO_TEXTO")]
        public string ComentarioTexto { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
    }
}
