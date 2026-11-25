
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Obra>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Obras)
                .HasForeignKey(o => o.IdCliente);

            modelBuilder.Entity<Obra>()
                .HasOne(o => o.Vendedor)
                .WithMany()
                .HasForeignKey(o => o.IdVendedor);

            modelBuilder.Entity<Medicao>()
                .HasOne(m => m.Obra)
                .WithMany(o => o.Medicoes)
                .HasForeignKey(m => m.IdObra);

            modelBuilder.Entity<Medicao>()
                .HasOne(m => m.Vendedor)
                .WithMany()
                .HasForeignKey(m => m.IdVendedor);

            modelBuilder.Entity<Instalacao>()
                .HasOne(i => i.Obra)
                .WithMany(o => o.Instalacoes)
                .HasForeignKey(i => i.IdObra)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Instalacao>()
                .HasOne(i => i.Montador)
                .WithMany()
                .HasForeignKey(i => i.IdMontador)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RegistroInstalacao>()
                .HasOne(r => r.Instalacao)
                .WithMany(i => i.Registros)
                .HasForeignKey(r => r.IdInstalacao)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FotoInstalacao>()
                .HasOne(f => f.RegistroInstalacao)
                .WithMany(r => r.Fotos)
                .HasForeignKey(f => f.IdRegistro)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
