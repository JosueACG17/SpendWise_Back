using Microsoft.EntityFrameworkCore;
using SpendWise.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Perfil> Perfiles { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Gasto> Gastos { get; set; }
    public DbSet<Presupuesto> Presupuestos { get; set; }
    public DbSet<Etiqueta> Etiquetas { get; set; }
    public DbSet<ErrorLogs> ErrorLogs { get; set; }
    public DbSet<Rol> Roles { get; set; } 
    public DbSet<Token> Tokens { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Rol>().HasData(
            new Rol { Id = 1, Nombre = "Administrador" },
            new Rol { Id = 2, Nombre = "Usuario" }
        );
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany()
            .HasForeignKey(u => u.RolId);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Usuario)
            .WithMany(u => u.Gastos)
            .HasForeignKey(g => g.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Categoria>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Categorias)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Presupuesto>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Presupuestos)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Etiqueta>()
            .HasOne(e => e.Usuario)
            .WithMany(u => u.Etiquetas)
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Gasto>()
            .HasOne(g => g.Categoria)
            .WithMany(c => c.Gastos)
            .HasForeignKey(g => g.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Presupuesto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Presupuestos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Perfil>()
            .HasOne(p => p.Usuario)
            .WithOne(u => u.Perfil)
            .HasForeignKey<Perfil>(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
    }


}
