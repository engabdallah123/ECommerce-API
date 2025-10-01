using E_Commerce_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Cart;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartServices cartServices;

        public CartController(CartServices cartServices)
        {
            this.cartServices = cartServices;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var cart = await cartServices.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string userId, CartItemDTO itemDTO)
        {
            await cartServices.AddToCartAsync(userId, itemDTO);
            return Ok("Product added to cart");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(string userId, int productId, int quantity)
        {
            await cartServices.UpdateQuantityAsync(userId, productId, quantity);
            return Ok("Quantity updated");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromCart(string userId, int productId)
        {
            await cartServices.RemoveFromCartAsync(userId, productId);
            return Ok("Product removed");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            await cartServices.ClearCartAsync(userId);
            return Ok("Cart cleared");
        }
    }
}
