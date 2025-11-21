using Microsoft.EntityFrameworkCore;
using ParqueoUPDS.Models;

namespace ParqueoUPDS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;
        public DbSet<SesionParqueo> SesionesParqueo { get; set; } = null!;
        public DbSet<Recompensa> Recompensas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== TABLA vehiculos =====
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.ToTable("vehiculos"); // nombre real de PostgreSQL (minúsculas)

                entity.HasKey(v => v.Id);

                entity.Property(v => v.Id).HasColumnName("id");
                entity.Property(v => v.Placa).HasColumnName("placa");
                entity.Property(v => v.Marca).HasColumnName("marca");
                entity.Property(v => v.Modelo).HasColumnName("modelo");
                entity.Property(v => v.HorasAcumuladas).HasColumnName("horasacumuladas");
                entity.Property(v => v.RowVersion).HasColumnName("rowversion");

                entity.HasIndex(v => v.Placa).IsUnique();

                // Relaciones
                entity.HasMany(v => v.Sesiones)
                    .WithOne(s => s.Vehiculo)
                    .HasForeignKey(s => s.VehiculoId);

                entity.HasMany(v => v.Recompensas)
                    .WithOne(r => r.Vehiculo)
                    .HasForeignKey(r => r.VehiculoId);
            });

            // ===== TABLA sesionesparqueo =====
            modelBuilder.Entity<SesionParqueo>(entity =>
            {
                entity.ToTable("sesionesparqueo"); // nombre en postgres: todo minúsculas

                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id).HasColumnName("id");
                entity.Property(s => s.VehiculoId).HasColumnName("vehiculoid");
                entity.Property(s => s.HoraEntrada).HasColumnName("horaentrada");
                entity.Property(s => s.HoraSalida).HasColumnName("horasalida");
                entity.Property(s => s.DuracionHoras).HasColumnName("duracionhoras");
                entity.Property(s => s.Estado).HasColumnName("estado");
            });

            // ===== TABLA recompensas =====
            modelBuilder.Entity<Recompensa>(entity =>
            {
                entity.ToTable("recompensas");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id).HasColumnName("id");
                entity.Property(r => r.VehiculoId).HasColumnName("vehiculoid");
                entity.Property(r => r.FechaEntrega).HasColumnName("fechaentrega");
                entity.Property(r => r.TipoPremio).HasColumnName("tipopremio");
                entity.Property(r => r.HorasAcumuladasAlMomento).HasColumnName("horasacumuladasalmomento");
            });
        }

    }
}
