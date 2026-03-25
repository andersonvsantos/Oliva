using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;
using Oliva.Models.Dtos.Product;

namespace Oliva.Service
{
    public class ProductService
    {
        private readonly AppDbContext _databaseContext;
        private readonly ProductCategoryService _productCategoryService;

        public ProductService(AppDbContext databaseContext, ProductCategoryService productCategoryService)
        {
            _databaseContext = databaseContext;
            _productCategoryService = productCategoryService;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _databaseContext.Products
                .Include(p => p.Categories)
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _databaseContext.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Product> CreateNewProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Color = productDto.Color?.ToList() ?? new List<string>(),
                Images = productDto.Images?.ToList() ?? new List<string>()
            };

            if (productDto.Categories != null)
            {
                foreach (var categoryName in productDto.Categories)
                {
                    var category = await _productCategoryService.GetProductCategoryByName(categoryName);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }
            }

            _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync();

            return product;
        }
    }
}