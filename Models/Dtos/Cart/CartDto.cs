using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oliva.Models.Dtos.Cart
{
    public class CartDto
    {
        public string UserUUID { get; set; }
        public List<CartItemDto> CartItens { get; set; } = new();
    }
}