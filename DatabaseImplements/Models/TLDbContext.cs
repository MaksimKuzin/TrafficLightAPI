using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplements.Models
{
    public class TLDbContext: DbContext
    {
        public virtual DbSet<Sequence> Sequences { get; set; }
        public virtual DbSet<Observation> Observations { get; set; }

        public TLDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=TrafficLight;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sequence>()
                .Navigation(s => s.Observations).AutoInclude();
        }
    }
}
