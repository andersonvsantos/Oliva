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
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductCategory> ProductCategories {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(product => product.Categories)
                .WithMany()
                .UsingEntity(joinTable => joinTable.ToTable("ProductProductCategories"));

            modelBuilder.Entity<ProductCategory>()
                .HasOne(productCategory => productCategory.Parent)
                .WithMany()
                .HasForeignKey(productCategory => productCategory.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(product => product.Variants)
                .WithOne(variant => variant.Product)
                .HasForeignKey(variant => variant.ProductId);
        }
    }
}