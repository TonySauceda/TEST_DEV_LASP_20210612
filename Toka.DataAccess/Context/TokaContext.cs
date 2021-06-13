using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toka.Core.Models;

namespace Toka.DataAccess
{
    public class TokaContext : DbContext
    {
        public TokaContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PersonasFisicas> PersonasFisicas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<EliminarPersonaFisicaResult> EliminarPersonaFisicaResult { get; set; }
        public DbSet<AgregarPersonaFisicaResult> AgregarPersonaFisicaResult { get; set; }
        public DbSet<ActualizarPersonaFisicaResult> ActualizarPersonaFisicaResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(e => e.HasKey(x => x.IdUsuario));
            modelBuilder.Entity<EliminarPersonaFisicaResult>(e => e.HasNoKey());
            modelBuilder.Entity<AgregarPersonaFisicaResult>(e => e.HasNoKey());
            modelBuilder.Entity<ActualizarPersonaFisicaResult>(e => e.HasNoKey());
        }
    }
}
