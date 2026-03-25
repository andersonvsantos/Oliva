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
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}