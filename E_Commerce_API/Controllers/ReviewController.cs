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
        UnitWork unitWork;
        public ReviewController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var review = unitWork.ReviewRepo.GetById(id);
            if (review == null)
                return NotFound();
            else
            {
                ReviewDTO reviewDTO = new ReviewDTO()
                {
                    Id = review.Id,
                    Subject = review.Subject,
                    Comment = review.Comment,
                    UserName = unitWork.db.Registers
                                .Where(u => u.Id == review.UserId)
                                .Select(u => u.FullName)
                                .FirstOrDefault(),
                    ProductName = unitWork.db.Products
                                .Where(p => p.Id == review.ProductId)
                                .Select(p => p.Name)
                                .FirstOrDefault()
                    
                };
                return Ok(reviewDTO);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] ReviewDTO reviewDTO)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review()
                {
                    Subject = reviewDTO.Subject,
                    Comment = reviewDTO.Comment,
                    UserId = unitWork.db.Registers
                                .Where(u => u.FullName == reviewDTO.UserName)
                                .Select(u => u.Id)
                                .FirstOrDefault(),
                    ProductId = unitWork.db.Products
                                .Where(p => p.Name == reviewDTO.ProductName)
                                .Select(p => p.Id)
                                .FirstOrDefault()
                };
                unitWork.ReviewRepo.Add(review);
                unitWork.Save();
                return Created("api/review", review);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReviewDTO reviewDTO)
        {
            if (ModelState.IsValid)
            {
                var review = unitWork.ReviewRepo.GetById(id);
                if (review == null)
                    return NotFound();
                else
                {
                    Review review1 = new Review()
                    {
                        Id = review.Id,
                        Subject = review.Subject,
                        Comment = review.Comment,
                        UserId = unitWork.db.Registers
                                .Where(u => u.FullName == reviewDTO.UserName)
                                .Select(u => u.Id)
                                .FirstOrDefault(),
                        ProductId = unitWork.db.Products
                                .Where(p => p.Name == reviewDTO.ProductName)
                                .Select(p => p.Id)
                                .FirstOrDefault()
                    };
                    
                    unitWork.ReviewRepo.Update(review, id);
                    unitWork.Save();
                    return Ok(review1);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = unitWork.ReviewRepo.GetById(id);
            if (review == null)
                return NotFound();
            else
            {
                unitWork.ReviewRepo.Delete(id);
                unitWork.Save();
                return NoContent();
            }
        }
    }
}
