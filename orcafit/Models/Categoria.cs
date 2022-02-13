using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    [Table("CATEGORIAS")]
    public class Categoria
    {
        [Key]
        [Column("NOMBRE")]
        public string Nombre { get; set; }
    }
}
