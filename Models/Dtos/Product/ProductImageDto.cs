using System.ComponentModel.DataAnnotations;

namespace Oliva.Models.Dtos.Product
{
    public class ProductImageDto
    {
        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
    }
}