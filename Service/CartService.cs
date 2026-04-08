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
                    var variant = await _productService.GetProductVariantById(cartItem.VariantId);
                    if (variant == null)
                    {
                        throw new Exception($"Variant with id {cartItem.VariantId} not found.");
                    }

                    if (cartItem.Quantity > variant.Stock)
                    {
                        throw new Exception($"Not enough stock for variant {variant.Name}. Available: {variant.Stock}");
                    }

                    var newCartItem = new CartItem
                    {
                        VariantId = variant.Id,
                        VariantName = variant.Name,
                        ProductPrice = variant.Product.Price,
                        Quantity = cartItem.Quantity,
                        Stock = variant.Stock
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

            var variant = await _productService.GetProductVariantById(cartItemDto.VariantId);

            if (variant == null)
            {
                throw new Exception("Variant not found.");
            }

            var existingItem = cartDb.Items.FirstOrDefault(item => item.VariantId == variant.Id);

            if (existingItem != null)
            {
                if (existingItem.Quantity + cartItemDto.Quantity > variant.Stock)
                {
                    throw new Exception($"Not enough stock for variant {variant.Name}. Available: {variant.Stock}");
                }
                existingItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                if (cartItemDto.Quantity > variant.Stock)
                {
                    throw new Exception($"Not enough stock for variant {variant.Name}. Available: {variant.Stock}");
                }
                var newCartItem = new CartItem
                {
                    VariantId = variant.Id,
                    VariantName = variant.Name,
                    ProductPrice = variant.Product.Price,
                    Quantity = cartItemDto.Quantity,
                    Stock = variant.Stock
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

            var variant = await _productService.GetProductVariantById(cartItemDto.VariantId);

            if (variant == null)
            {
                throw new Exception("Variant not found.");
            }

            var existingItem = cartDb.Items.FirstOrDefault(item => item.VariantId == variant.Id);

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