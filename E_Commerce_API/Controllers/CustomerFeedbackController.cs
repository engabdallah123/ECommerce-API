using Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTO;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFeedbackController : ControllerBase
    {
        private readonly UnitWork unitWork;

        public CustomerFeedbackController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet]
        public async Task<ActionResult<List<CustomerFeedbackDTO>>> GetFeedbacks()
        {
            var feeds = await unitWork.CustomerFeedbackRepo.GetAllAsync();
            var feedBacks = new List<CustomerFeedbackDTO>();
            if (feeds == null)
            {
                return NotFound("Not Found Any Messages");
            }
            else
            {

                foreach (var feed in feeds)
                {
                    var feedBack = new CustomerFeedbackDTO()
                    {
                        Id = feed.Id,
                        CustomerName = feed.CustomerName,
                        CustomerEmail = feed.CustomerEmail,
                        Message = feed.Message,
                        Date = feed.Date,
                        Responded = feed.Responded,
                        Type = feed.Type,
                    };
                    feedBacks.Add(feedBack);
                }
            }
            return Ok(feedBacks);
        }
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetById(int id)
        {
            var feedBack = await unitWork.CustomerFeedbackRepo.GetByIdAsync(id);
            if (feedBack == null)
                return BadRequest();
            else
            {
                var feedBackDto = new CustomerFeedbackDTO()
                {
                    Id = feedBack.Id,
                    CustomerName = feedBack.CustomerName,
                    CustomerEmail = feedBack.CustomerEmail,
                    Message = feedBack.Message,
                    Type = feedBack.Type,
                    Responded = feedBack.Responded,
                    Date = feedBack.Date

                };
                return Ok(feedBackDto);
            }
        }
        [HttpPost]
        public async Task<ActionResult<CustomerFeedbackDTO>> PostFeedback(CustomerFeedbackDTO feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var feeds = new CustomerFeedback()
            {
                CustomerName = feedbackDto.CustomerName,
                CustomerEmail = feedbackDto.CustomerEmail,
                Message = feedbackDto.Message,
                Type = feedbackDto.Type,
            };
            unitWork.CustomerFeedbackRepo.Add(feeds);
            unitWork.CustomerFeedbackRepo.Save();

            return Created();

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFeedBack(int id, [FromBody] CustomerFeedbackDTO feedbackDTO)
        {
            var feddBack = await unitWork.CustomerFeedbackRepo.GetByIdAsync(id);
            if (feddBack == null)
                return NotFound();

            
            feddBack.CustomerName = feedbackDTO.CustomerName;
            feddBack.CustomerEmail = feedbackDTO.CustomerEmail;
            feddBack.Message = feedbackDTO.Message;
            feddBack.Type = feedbackDTO.Type;
            feddBack.Responded = feedbackDTO.Responded;

            unitWork.CustomerFeedbackRepo.Update(feddBack, id);
             unitWork.CustomerFeedbackRepo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedBack(int id)
        {
            var feddBack = await unitWork.CustomerFeedbackRepo.GetByIdAsync(id);
            if (feddBack == null)
                return BadRequest();

            unitWork.CustomerFeedbackRepo.Delete(id);
            unitWork.CustomerFeedbackRepo.Save();
            return NoContent();
        }

    }
}
