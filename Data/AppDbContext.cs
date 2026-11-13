using Microsoft.EntityFrameworkCore;
using S.A_Esquadrias.Models;

namespace S.A_Esquadrias.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Obra> Obra { get; set; }
        public DbSet<Medicao> Medicao { get; set; }
        public DbSet<Instalacao> Instalacao { get; set; }
        public DbSet<RegistroInstalacao> RegistroInstalacao { get; set; }
        public DbSet<FotoInstalacao> FotoInstalacao { get; set; }
    }
}
