using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Order;
using Models.DTO.Wallet;
using System.Threading.Tasks;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await orderService.GetAllOrder();
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
            
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           var result = await orderService.GetOrderById(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        [HttpPost]
        public IActionResult Post(OrderDTO orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    orderService.PostOrder(orderDTO);
                    return CreatedAtAction(nameof(GetById), new { id = orderDTO.Id }, orderDTO);
                }
                else
                {
                    return BadRequest("Unsupported payment method.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public IActionResult Modify(int id, OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                orderService.UpdateOrder(id, orderDTO);
                    return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                orderService.DeleteOrder(id);
                return NoContent();
            }
            else if (id == 0)
            {
                return BadRequest("Id cannot be zero.");
            }
            else
                return NotFound();


            
        }

    }
}
