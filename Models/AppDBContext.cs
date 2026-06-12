using Microsoft.EntityFrameworkCore;

namespace ComentariosAnonimos.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Comentario> Comentarios => Set<Comentario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.ToTable("Comentarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Texto).IsRequired(false);
            entity.Property(e => e.NombreUsuario).IsRequired();
            entity.Property(e => e.MediaUrl).IsRequired(false);
            entity.Property(e => e.Fecha).HasDefaultValueSql("NOW()");

            entity.HasMany<Comentario>()
                  .WithOne()
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
