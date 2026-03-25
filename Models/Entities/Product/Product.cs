using System.ComponentModel.DataAnnotations;
using Oliva.Models.Entities.Product;

namespace Oliva.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required!")]
        public decimal Price { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Color is required!")]
        [MaxLength(150)]
        public List<string> Color { get; set; } = new();
        public List<ProductCategory> Categories { get; set; } = new();
        public List<string> Images { get; set; } = new();
    }
}