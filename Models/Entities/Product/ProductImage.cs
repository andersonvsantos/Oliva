using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oliva.Models;

namespace Oliva.Models.Entities.Product
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ImageUrl is required!")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "ProductId is required!")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Oliva.Models.Product Product { get; set; }
    }
}