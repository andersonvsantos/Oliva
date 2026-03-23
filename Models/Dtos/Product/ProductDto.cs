namespace Oliva.Models.Dtos.Product
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string[]? Categories { get; set; }
        public string[]? Images { get; set; }
    }
}