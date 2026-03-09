using System.ComponentModel.DataAnnotations;

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
        public List<ProductImage> Images { get; set; } = new();
    }
}