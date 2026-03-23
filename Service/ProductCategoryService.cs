using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models.Dtos.Product;
using Oliva.Models.Entities.Product;

namespace Oliva.Service
{
    public class ProductCategoryService
    {
        private readonly AppDbContext _databaseContext;

        public ProductCategoryService(AppDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync()
        {
            return await _databaseContext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory?> GetProductCategoryById(int productCategoryId)
        {
            return await _databaseContext.ProductCategories.FindAsync(productCategoryId);
        }

        public async Task<ProductCategory?> GetProductCategoryByName(string categoryName)
        {
            return await _databaseContext.ProductCategories
                .FirstOrDefaultAsync(productCategory => productCategory.Name == categoryName);
        }

        public async Task<ProductCategory> CreateNewProductCategory(ProductCategoryDto categoryDto)
        {
            var categoryDb = await GetProductCategoryByName(categoryDto.Name);
            ProductCategory? parent = null;

            if (categoryDb != null)
            {
                throw new Exception("There is already a created category with this name.");
            }

            if (categoryDto.ParentName != null)
            {
                parent = await _databaseContext.ProductCategories
                    .FirstOrDefaultAsync(category => category.Name == categoryDto.ParentName);
                
                if (parent == null)
                    throw new Exception($"Parent category not found.");
            }

            var newCategory = new ProductCategory
            {
                Name = categoryDto.Name,
                Parent = parent
            };

            _databaseContext.ProductCategories.Add(newCategory);
            await _databaseContext.SaveChangesAsync();

            return newCategory;
        }

        public async Task UpdateProductCategoryAsync(int categoryId, ProductCategoryDto categoryDto)
        {
            var categoryDb = await GetProductCategoryById(categoryId);
            ProductCategory? parent = null;
            
            if (categoryDb == null)
            {
                throw new Exception("Category not found for update");
            }

            if (categoryDto.ParentName != null)
            {
                parent = await _databaseContext.ProductCategories
                    .FirstOrDefaultAsync(category => category.Name == categoryDto.ParentName);
                
                if (parent == null)
                    throw new Exception($"Parent category not found.");
            }

            _databaseContext.Entry(categoryDb).CurrentValues.SetValues(categoryDto);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteProductCategoryAsync(int categoryId)
        {
            var categoryDb = await _databaseContext.ProductCategories.FindAsync(categoryId);
            
            if (categoryDb == null) 
            {
                throw new Exception("Category not found for deleting.");
            }

            _databaseContext.ProductCategories.Remove(categoryDb);
            await _databaseContext.SaveChangesAsync();
        }
    }
}