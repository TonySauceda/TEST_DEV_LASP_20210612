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
        public DbSet<Resultado> Resultado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resultado>(e => e.HasNoKey());
        }
    }
}
