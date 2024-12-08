using Microsoft.EntityFrameworkCore;
using SistemaMultas.Models;

using Microsoft.EntityFrameworkCore;
using SistemaMultas.Models;

namespace SistemaMultas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Multa> Multas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Concepto> Conceptos { get; set; }
    }
}