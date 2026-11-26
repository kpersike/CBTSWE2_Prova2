using CBTSWE2_Prova2.Core;
using Microsoft.EntityFrameworkCore;

namespace CBTSWE2_Prova2.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Nome)
                .IsUnique();

            modelBuilder.Entity<Produto>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(p => p.IdUsuarioCadastro)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Produto>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(p => p.IdUsuarioUpdate)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}