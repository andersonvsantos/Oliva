namespace Oliva.Models.Entities.Cart
{
    public class CartItem
    {
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
    }
}