using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Cart;
using System.Threading.Tasks;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        UnitWork UnitWork;
        public CartController(UnitWork unitWork)
        {
            this.UnitWork = unitWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await UnitWork.CartRepo.GetAllAsync();
            if (carts == null)
                return NotFound();
            else
            {
                List<CartDTO> cartDTOs = new List<CartDTO>();
                foreach (Cart cart in carts)
                {
                    CartDTO cartDTO = new CartDTO()
                    {
                        ProductId = cart.ProductId,
                        UserName = cart.User != null ? cart.User.FullName : null, // Fixed issue here
                        Quantity = cart.Quantity,
                        TotalPrice = cart.TotalPrice
                    };
                    cartDTOs.Add(cartDTO);
                }
                return Ok(cartDTOs);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await UnitWork.CartRepo.GetByIdAsync(id);
            if (cart == null)
                return NotFound();
            else
            {
                CartDTO cartDTO = new CartDTO()
                {
                    ProductId = cart.ProductId,
                    UserName = cart.User != null ? cart.User.FullName : null, // Fixed issue here
                    Quantity = cart.Quantity,
                    TotalPrice = cart.TotalPrice
                };
                return Ok(cartDTO);
            }
        }
        [HttpPost]
        public IActionResult Add([FromBody] CartDTO cartDTO)
        {
            if (cartDTO == null)
                return BadRequest();
            else
            {
               
                Cart cart = new Cart()
                {
                    TotalPrice = (decimal)cartDTO.TotalPrice,
                    ProductId = cartDTO.ProductId,
                    UserId = cartDTO.UserId,
                    Quantity = cartDTO.Quantity ,
                   
                };
                UnitWork.CartRepo.Add(cart);
                UnitWork.Save();
                return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CartDTO cartDTO)
        {
            if (cartDTO == null)
                return BadRequest();
            else
            {
                Cart cart = new Cart()
                {
                    Id = id,
                    ProductId = cartDTO.ProductId,
                    UserId = cartDTO.UserId,
                    Quantity = cartDTO.Quantity,
                    TotalPrice =(decimal) cartDTO.TotalPrice
                };
                UnitWork.CartRepo.Update(cart, id);
                UnitWork.Save();
                return NoContent();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await UnitWork.CartRepo.GetByIdAsync(id);
            if (cart == null)
                return NotFound();
            else
            {
                UnitWork.CartRepo.Delete(id);
                UnitWork.Save();
                return NoContent();
            }
        }
    }
}
