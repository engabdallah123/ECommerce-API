using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Review;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
         private readonly ReviewService reviewService;
         public ReviewController(ReviewService reviewService)
        {
            this.reviewService = reviewService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await reviewService.GetById(id);
            if (review == null)
                return NotFound();
            else
                return Ok(review);

        }
        [HttpPost]
        public IActionResult Post([FromBody] ReviewDTO reviewDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reviewService.Post(reviewDTO);
                    return CreatedAtAction(nameof(GetById), new { id = reviewDTO.Id }, reviewDTO);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReviewDTO reviewDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    reviewService.Post(reviewDTO);
                    return NoContent();
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            reviewService.Delete(id);
            return Ok(new { message = "Review deleted successfully." });
        }
    }
}
