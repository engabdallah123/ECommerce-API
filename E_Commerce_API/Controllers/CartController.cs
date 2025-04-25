using E_Commerce_API.Service.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Cart;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpGet("GetCart/{userId}")]
        public IActionResult GetCart(string userId)
        {
            var cart = cartService.GetCart(userId);
            return Ok(cart);
        }
        [HttpPost("AddToCart/{userId}")]
        public IActionResult AddToCart(string userId, [FromBody] CartItemDTO cartItem)
        {
            cartService.AddToCart(userId, cartItem);
            return Ok("Done Adding");
        }
        [HttpDelete("DeletItem/{userId}/{productId}")]
        public IActionResult RemoveFromCart(string userId, int productId)
        {
            cartService.RemoveFromCart(userId, productId);
            return Ok("Sucssfuly Deleting");
        }
        [HttpDelete("ClearCart/{userId}")]
        public IActionResult ClearCart(string userId)
        {
            cartService.ClearCart(userId);
            return Ok("All cart is cleared");
        }
    }
}
