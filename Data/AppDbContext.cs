using Microsoft.EntityFrameworkCore;
using Oliva.Models;

namespace Oliva.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}
        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
    }
}