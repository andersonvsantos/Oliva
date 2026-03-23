using System.ComponentModel.DataAnnotations;

namespace Oliva.Models.Entities.Product
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public ProductCategory? Parent { get; set; }
    }
}