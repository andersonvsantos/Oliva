using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models.Dtos.Cart;
using Oliva.Models.Entities.Cart;

namespace Oliva.Service
{
    public class CartService
    {
        private readonly AppDbContext _databaseContext;

        private readonly ProductService _productService;

        public CartService(AppDbContext databaseContext, ProductService productService)
        {
            _databaseContext = databaseContext;
            _productService = productService;
        }

        public async Task<Cart?> GetCartByUUIDAsync(string UUID)
        {
            return await _databaseContext.Cart.FirstOrDefaultAsync(cart => cart.UserUUID == UUID);
        }

        public async Task<Cart> CreateUserCartAsync(CartDto cartDto)
        {
            var cartDb = await _databaseContext.Cart.FirstOrDefaultAsync(cart => cart.UserUUID == cartDto.UserUUID);
            
            if(cartDb != null)
            {
                throw new Exception("There is already a created cart for this user.");
            }

            var cart = new Cart
            {
                Id =  Guid.NewGuid().ToString(),
                UserUUID = cartDto.UserUUID
            };

            if(cartDto.CartItens != null && cartDto.CartItens.Any())
            {
                foreach(var cartItem in cartDto.CartItens)
                {
                    var product = await _productService.GetProductById(cartItem.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with id {cartItem.ProductId} not found.");
                    }

                    var newCartItem = new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        Quantity = cartItem.Quantity
                    };

                    cart.Items.Add(newCartItem);
                }
            }
            _databaseContext.Cart.Add(cart);
            await _databaseContext.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> AddCartItemAsync(string UUID, CartItemDto cartItemDto)
        {
            var cartDb = await _databaseContext.Cart.FirstOrDefaultAsync(cart => cart.UserUUID == UUID);

            if (cartDb == null)
            {
                throw new Exception("There isn't a created cart for this user.");
            }

            var product = await _productService.GetProductById(cartItemDto.ProductId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var existingItem = cartDb.Items.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = cartItemDto.Quantity
                };
                cartDb.Items.Add(newCartItem);
            }

            await _databaseContext.SaveChangesAsync();

            return cartDb;
        }

        public async Task<Cart> RemoveCartItemAsync(string UUID, CartItemDto cartItemDto)
        {
            var cartDb = await _databaseContext.Cart.FirstOrDefaultAsync(cart => cart.UserUUID == UUID);

            if (cartDb == null)
            {
                throw new Exception("There isn't a created cart for this user.");
            }

            var product = await _productService.GetProductById(cartItemDto.ProductId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var existingItem = cartDb.Items.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity > cartItemDto.Quantity)
                {
                    existingItem.Quantity -= cartItemDto.Quantity;
                }
                else
                {
                    cartDb.Items.Remove(existingItem);
                }
            }
            else
            {
                throw new Exception("This product is not in your cart.");
            }

            await _databaseContext.SaveChangesAsync();

            return cartDb;
        }

        public async Task<Cart> ClearCartAsync(string UUID)
        {
            var cartDb = await _databaseContext.Cart.FirstOrDefaultAsync(cart => cart.UserUUID == UUID);

            if (cartDb == null)
            {
                throw new Exception("There isn't a created cart for this user.");
            }

            cartDb.Items.Clear();

            await _databaseContext.SaveChangesAsync();

            return cartDb;
        }
    }
}