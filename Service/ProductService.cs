using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;
using Oliva.Models.Dtos.Product;
using Oliva.Models.Entities.Product;

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
                .Include(product => product.Categories)
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int productId)
        {
            return await _databaseContext.Products
                .Include(product => product.Categories)
                .FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task<Product> CreateNewProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Images = productDto.Images?.ToList() ?? new List<string>()
            };

            if (productDto.Variants != null)
            {
                foreach (var variantDto in productDto.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Name = variantDto.Name,
                        Stock = variantDto.Stock
                    };

                    product.Variants.Add(variant);
                }
            }

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

        public async Task UpdateProductAsync(int productId, ProductDto productDto)
        {
            var productDb = await _databaseContext.Products
                .Include(product => product.Categories)
                .FirstOrDefaultAsync(product => product.Id == productId);

            if (productDb == null)
            {
                throw new Exception("Product not found for update.");
            }

            var updatedProduct = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Images = productDto.Images?.ToList() ?? new List<string>()
            };

            if (productDto.Variants != null)
            {
                foreach (var variantDto in productDto.Variants)
                {
                    var variant = new ProductVariant
                    {
                        Name = variantDto.Name,
                        Stock = variantDto.Stock
                    };

                    updatedProduct.Variants.Add(variant);
                }
            }

            if (productDto.Categories != null)
            {
                foreach (var categoryName in productDto.Categories)
                {
                    var category = await _productCategoryService.GetProductCategoryByName(categoryName);
                    if (category != null)
                    {
                        updatedProduct.Categories.Add(category);
                    }
                }
            }

            _databaseContext.Entry(productDb).CurrentValues.SetValues(updatedProduct);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var productDb = await _databaseContext.Products
                .Include(product => product.Categories)
                .FirstOrDefaultAsync(product => product.Id == productId);

            if (productDb == null)
            {
                throw new Exception("Product not found for deleting.");
            }

            _databaseContext.Products.Remove(productDb);
            await _databaseContext.SaveChangesAsync();
        }
    }
}