using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oliva.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Url is required!")]
        public string ImageUrl { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}