using Microsoft.AspNetCore.Mvc;
using Oliva.Models.Dtos.Cart;
using Oliva.Service;

namespace Oliva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{UUID}")]
        public async Task<IActionResult> GetCartByUUID(string UUID)
        {
            var cart = await _cartService.GetCartByUUIDAsync(UUID);
            if (cart == null)
            {
                return NotFound("Cart not found.");
            }

            return Ok(cart);
        }

        [HttpPost("{UUID}")]
        public async Task<IActionResult> CreateCart(string UUID, [FromBody] CartDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                cartDto.UserUUID = UUID;
                var cart = await _cartService.CreateUserCartAsync(cartDto);
                return CreatedAtAction(nameof(GetCartByUUID), new { UUID = cart.UserUUID }, cart);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost("{UUID}/items")]
        public async Task<IActionResult> AddCartItem(string UUID, [FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cart = await _cartService.AddCartItemAsync(UUID, cartItemDto);
                return Ok(cart);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{UUID}/items")]
        public async Task<IActionResult> RemoveCartItem(string UUID, [FromBody] CartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cart = await _cartService.RemoveCartItemAsync(UUID, cartItemDto);
                return Ok(cart);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("{UUID}")]
        public async Task<IActionResult> ClearCart(string UUID)
        {
            try
            {
                var cart = await _cartService.ClearCartAsync(UUID);
                return Ok(cart);
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
