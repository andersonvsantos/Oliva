using Microsoft.AspNetCore.Mvc;
using Oliva.Models.Dtos.Product;
using Oliva.Service;

namespace Oliva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly ProductCategoryService _categoryService;

        public ProductCategoriesController(ProductCategoryService productCategoryService)
        {
            _categoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductCategories()
        {
            var categoriesList = await _categoryService.GetAllProductCategoriesAsync();
            return Ok(categoriesList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategoryById(int id)
        {
            var dbCategory = await _categoryService.GetProductCategoryById(id);
            
            if(dbCategory == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(dbCategory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory([FromBody] ProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCategory = await _categoryService.CreateNewProductCategoryAsync(categoryDto);

                return CreatedAtAction(nameof(GetProductCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(int id, [FromBody] ProductCategoryDto categoryDto)
        {
            try
            {
                await _categoryService.UpdateProductCategoryAsync(id, categoryDto);
                return Ok("Category updated with success."); 
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                await _categoryService.DeleteProductCategoryAsync(id);
                return Ok("Category deleted with success.");
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }
    }
}