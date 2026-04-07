using System.ComponentModel.DataAnnotations;

namespace Oliva.Models.Entities.Cart
{
    public class Cart
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "UUID is required!")]
        public string UserUUID { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }
}