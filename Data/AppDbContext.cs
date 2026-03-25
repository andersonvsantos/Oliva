using Microsoft.EntityFrameworkCore;
using Oliva.Models;
using Oliva.Models.Entities.Product;

namespace Oliva.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<ProductCategory> ProductCategories {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between Product and ProductCategory
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany()
                .UsingEntity(j => j.ToTable("ProductProductCategories"));

            // Configure self-referencing relationship for ProductCategory
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Parent)
                .WithMany()
                .HasForeignKey(pc => pc.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}