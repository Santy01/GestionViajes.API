using Microsoft.EntityFrameworkCore;
using GestionViajes.API.Models;

namespace GestionViajes.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Turista> Turistas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para Destino
            modelBuilder.Entity<Destino>(entity =>
            {
                entity.HasKey(e => e.DestinoId);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Pais).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Costo).HasColumnType("decimal(18,2)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                // Índice para mejorar búsquedas
                entity.HasIndex(e => e.Nombre);
                entity.HasIndex(e => e.Pais);
            });

            // Configuración para Turista
            modelBuilder.Entity<Turista>(entity =>
            {
                entity.HasKey(e => e.TuristaId);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETUTCDATE()");

                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Índices para mejorar búsquedas
                entity.HasIndex(e => e.Apellido);
            });

            // Configuración para Reserva
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.ReservaId);
                entity.Property(e => e.CantidadPersonas).HasDefaultValue(1);
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Turista)
                      .WithMany()
                      .HasForeignKey(e => e.TuristaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Destino)
                      .WithMany()
                      .HasForeignKey(e => e.DestinoId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Índices para consultas comunes
                entity.HasIndex(e => e.TuristaId);
                entity.HasIndex(e => e.DestinoId);
                entity.HasIndex(e => new { e.FechaInicio, e.FechaFin });

                // Check constraint para validar rango de fechas (solo SQL Server)
                entity.ToTable(tb => tb.HasCheckConstraint("CK_Reserva_Fechas", "FechaFin > FechaInicio"));
            });

            // Datos semilla (seed data)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Datos iniciales para Destinos
            modelBuilder.Entity<Destino>().HasData(
                new Destino
                {
                    DestinoId = 1,
                    Nombre = "Machu Picchu",
                    Pais = "Perú",
                    Descripcion = "Antigua ciudad inca en la cordillera de los Andes",
                    Costo = 250.00m,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Destino
                {
                    DestinoId = 2,
                    Nombre = "Cristo Redentor",
                    Pais = "Brasil",
                    Descripcion = "Icónica estatua de Cristo en Río de Janeiro",
                    Costo = 180.00m,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Destino
                {
                    DestinoId = 3,
                    Nombre = "Cataratas del Iguazú",
                    Pais = "Argentina",
                    Descripcion = "Majestuosas cascadas en la frontera argentino-brasileña",
                    Costo = 150.00m,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Destino
                {
                    DestinoId = 4,
                    Nombre = "Chichén Itzá",
                    Pais = "México",
                    Descripcion = "Antigua ciudad maya y una de las nuevas siete maravillas del mundo",
                    Costo = 200.00m,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Destino
                {
                    DestinoId = 5,
                    Nombre = "Torres del Paine",
                    Pais = "Chile",
                    Descripcion = "Parque nacional con impresionantes formaciones rocosas en la Patagonia",
                    Costo = 300.00m,
                    FechaCreacion = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Datos iniciales para Turistas
            modelBuilder.Entity<Turista>().HasData(
                new Turista
                {
                    TuristaId = 1,
                    Nombre = "Carlos",
                    Apellido = "Mendoza",
                    Email = "carlos.mendoza@example.com",
                    Telefono = "+51 987654321",
                    FechaRegistro = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Turista
                {
                    TuristaId = 2,
                    Nombre = "María",
                    Apellido = "González",
                    Email = "maria.gonzalez@example.com",
                    Telefono = "+54 9 11 1234-5678",
                    FechaRegistro = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new Turista
                {
                    TuristaId = 3,
                    Nombre = "José",
                    Apellido = "Silva",
                    Email = "jose.silva@example.com",
                    Telefono = "+55 11 98765-4321",
                    FechaRegistro = new DateTime(2024, 1, 25, 0, 0, 0, DateTimeKind.Utc)
                },
                new Turista
                {
                    TuristaId = 4,
                    Nombre = "Ana",
                    Apellido = "López",
                    Email = "ana.lopez@example.com",
                    Telefono = "+52 55 1234-5678",
                    FechaRegistro = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Turista
                {
                    TuristaId = 5,
                    Nombre = "Pedro",
                    Apellido = "Rodríguez",
                    Email = "pedro.rodriguez@example.com",
                    Telefono = "+56 9 8765-4321",
                    FechaRegistro = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Datos iniciales para Reservas (coherentes con Turistas y Destinos existentes)
            modelBuilder.Entity<Reserva>().HasData(
                new Reserva
                {
                    ReservaId = 1,
                    TuristaId = 1,
                    DestinoId = 1,
                    FechaInicio = new DateTime(2024, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                    FechaFin = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    CantidadPersonas = 2,
                    Total = 500.00m,
                    FechaCreacion = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Reserva
                {
                    ReservaId = 2,
                    TuristaId = 2,
                    DestinoId = 3,
                    FechaInicio = new DateTime(2024, 4, 5, 0, 0, 0, DateTimeKind.Utc),
                    FechaFin = new DateTime(2024, 4, 8, 0, 0, 0, DateTimeKind.Utc),
                    CantidadPersonas = 1,
                    Total = 150.00m,
                    FechaCreacion = new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Destino || e.Entity is Turista || e.Entity is Reserva)
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Destino destino)
                {
                    destino.FechaActualizacion = DateTime.UtcNow;
                }
                else if (entry.Entity is Turista turista)
                {
                    turista.FechaActualizacion = DateTime.UtcNow;
                }
                else if (entry.Entity is Reserva reserva)
                {
                    reserva.FechaActualizacion = DateTime.UtcNow;
                }
            }
        }
    }
}