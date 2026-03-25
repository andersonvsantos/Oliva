using Microsoft.AspNetCore.Mvc;
using Oliva.Models.Dtos.Product;
using Oliva.Service;

namespace Oliva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly ProductImageService _productImageService;

        public ProductImagesController(ProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductImages()
        {
            var images = await _productImageService.GetAllProductImagesAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImageById(int id)
        {
            var image = await _productImageService.GetProductImageByIdAsync(id);
            if (image == null)
                return NotFound("Product image not found.");

            return Ok(image);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var images = await _productImageService.GetProductImagesByProductIdAsync(productId);
            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage([FromBody] ProductImageDto imageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var image = await _productImageService.CreateProductImageAsync(imageDto);
                return CreatedAtAction(nameof(GetProductImageById), new { id = image.Id }, image);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(int id, [FromBody] ProductImageDto imageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productImageService.UpdateProductImageAsync(id, imageDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            try
            {
                await _productImageService.DeleteProductImageAsync(id);
                return Ok("Product image deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}