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
                .Where(e => e.Entity is Destino || e.Entity is Turista)
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
            }
        }
    }
}