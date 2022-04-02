using Introduccion.NET.Ejercicio.Entity.DbSet;
using Microsoft.EntityFrameworkCore;

namespace Introduccion.NET.Ejercicio.Repository
{
    public class PracticaDBContext : DbContext
    {

        #region DbSet Instance

        public DbSet<Calculator> Calculators { get; set; }
        
        public DbSet<Operation> Operations { get; set; }



        #endregion


        #region Constructor
        public PracticaDBContext(DbContextOptions<PracticaDBContext> options) : base(options)
        {
        }

        protected PracticaDBContext()
        {
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

}