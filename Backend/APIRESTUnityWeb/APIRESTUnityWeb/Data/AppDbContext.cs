using Microsoft.EntityFrameworkCore;
using APIRESTUnityWeb.Models;

namespace APIRESTUnityWeb.Data
{
    // Representa el contexto de base de datos y permite interactuar con PostgreSQL usando EF Core
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones del contexto (cadena de conexión, proveedor, etc.)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet que representa la tabla 'usuario' en la base de datos
        public DbSet<User> Users { get; set; }

        // Método opcional para configuraciones adicionales con Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("usuario");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("id_usuario");
                entity.Property(e => e.FirstName).HasColumnName("nombre");
                entity.Property(e => e.LastName).HasColumnName("apellido");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("contrasena");
                entity.Property(e => e.RegistrationDate).HasColumnName("fecha_registro");
            });
        }
    }
}
