using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models.Entities.Product;
using Oliva.Models.Dtos.Product;

namespace Oliva.Service
{
    public class ProductImageService
    {
        private readonly AppDbContext _databaseContext;

        public ProductImageService(AppDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<ProductImage>> GetAllProductImagesAsync()
        {
            return await _databaseContext.ProductImages.ToListAsync();
        }

        public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
        {
            return await _databaseContext.ProductImages.FindAsync(imageId);
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(int productId)
        {
            return await _databaseContext.ProductImages
                .Where(img => img.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductImage> CreateProductImageAsync(ProductImageDto imageDto)
        {
            var productExists = await _databaseContext.Products.AnyAsync(p => p.Id == imageDto.ProductId);
            if (!productExists)
                throw new Exception("Product not found.");

            var newImage = new ProductImage
            {
                ImageUrl = imageDto.ImageUrl,
                ProductId = imageDto.ProductId
            };

            _databaseContext.ProductImages.Add(newImage);
            await _databaseContext.SaveChangesAsync();

            return newImage;
        }

        public async Task UpdateProductImageAsync(int imageId, ProductImageDto imageDto)
        {
            var image = await _databaseContext.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new Exception("Product image not found.");

            var productExists = await _databaseContext.Products.AnyAsync(p => p.Id == imageDto.ProductId);
            if (!productExists)
                throw new Exception("Product not found.");

            image.ImageUrl = imageDto.ImageUrl;
            image.ProductId = imageDto.ProductId;

            _databaseContext.ProductImages.Update(image);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteProductImageAsync(int imageId)
        {
            var image = await _databaseContext.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new Exception("Product image not found.");

            _databaseContext.ProductImages.Remove(image);
            await _databaseContext.SaveChangesAsync();
        }
    }
}