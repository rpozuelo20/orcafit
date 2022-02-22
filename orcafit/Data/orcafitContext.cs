using Microsoft.EntityFrameworkCore;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Data
{
    public class orcafitContext:DbContext
    {
        public orcafitContext
            (DbContextOptions<orcafitContext> options) : base(options) { }

        //  Inyeccion de los modelos:
        public DbSet<RutinaComenzada> RutinasComenzadas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Rutina> Rutinas { get; set; }
    }
}
