using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Models
{
    [Table("EMAILS")]
    public class Email
    {
        [Key]
        [Column("EMAIL")]
        public string CorreoElectronico { get; set; }
    }
}
