using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;

namespace Oliva.Service
{
    public class ProductService
    {
        private readonly AppDbContext _databaseContext;

        public ProductService(AppDbContext databaseContext)
        {
            _databaseContext= databaseContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _databaseContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _databaseContext.Products.FindAsync(productId);
        }
    }
}