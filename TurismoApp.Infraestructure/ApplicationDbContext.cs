using Microsoft.EntityFrameworkCore;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recorrido>()
                .HasOne(r => r.CiudadOrigen)
                .WithMany()
                .HasForeignKey(r => r.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Recorrido>()
                .HasOne(r => r.CiudadDestino)
                .WithMany()
                .HasForeignKey(r => r.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecorridoCliente>()
                .HasKey(rc => new { rc.RecorridoId, rc.ClienteId });

            modelBuilder.Entity<RecorridoCliente>()
                .HasOne(rc => rc.Recorrido)
                .WithMany(r => r.RecorridoClientes)
                .HasForeignKey(rc => rc.RecorridoId);

            modelBuilder.Entity<RecorridoCliente>()
                .HasOne(rc => rc.Cliente)
                .WithMany(c => c.RecorridoClientes)
                .HasForeignKey(rc => rc.ClienteId);
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Recorrido> Recorridos { get; set; }
        public DbSet<RecorridoCliente> RecorridoClientes { get; set; }
    }
}
