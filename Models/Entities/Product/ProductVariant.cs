using Oliva.Models;

namespace Oliva.Models.Entities.Product
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}