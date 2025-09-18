using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Review;
using Models.DTO.Wallet;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WalletController : ControllerBase
    {
        private readonly WalletService walletService;
        public WalletController(WalletService walletService)
        {
            this.walletService = walletService;
        }
        [HttpGet("GetWallet/{username}")]
        public IActionResult InskAboutYourBalance(string username)
        {
            var result = walletService.GetWalletByUserName(username);
            if (result == null) 
                return NotFound();
            return Ok(new { data = result });

        }

        [HttpPut("AddMony/{id}")]
       
        public IActionResult AddMoney(int id,[FromBody] WalletDTO walletDTO)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    walletService.AddMoney(id, walletDTO);
                    return Ok("Mony is added successfully");
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
        [HttpDelete("DeleteWallet/{id}")]
        public IActionResult DeleteYourAccount(int id)
        {
            walletService.DeletWallet(id);
            return Ok(new { message = "Your account has been deleted successfully." });
        }
    }
}
