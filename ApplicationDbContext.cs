using ApiSeries.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiSeries
{
    public class ApplicationDbContext: DbContext 
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Serie> Series { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
