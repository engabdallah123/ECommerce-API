using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;
using Models.DTO.Wallet;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        UnitWork unitWork;
        public WalletController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        [HttpGet("GetWallet/{username}")]
        public IActionResult InskAboutYourBalance(string username)
        {
            var wallet = unitWork.db.Wallets.FirstOrDefault(w => w.UserName == username);
            if (wallet == null)
                return NotFound("Wallet not found");
            else
            {
                WalletDTO walletDTO = new WalletDTO()
                {

                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    UserName = wallet.UserName
                };
                return Ok(walletDTO);
            }

        }

        [HttpPut("AddMony/{id}")]
       
        public IActionResult AddMoney(int id,[FromBody] WalletDTO walletDTO)
        {
            if (ModelState.IsValid)
            {

                var wallet = unitWork.WalletRepo.GetById(id);
                if (wallet == null)
                    return NotFound("Wallet not found");
                else
                {
                    // Update the wallet's balance directly
                    wallet.Balance += walletDTO.Balance ?? 0; // Use null-coalescing operator to handle null values
                    unitWork.WalletRepo.Update(wallet, id);
                    unitWork.Save();
                    return Ok("Money added successfully");
                }
            }
            return BadRequest("Invalid data");
        }
        [HttpDelete("DeleteWallet/{id}")]
        public IActionResult DeleteYourAccount(int id)
        {
            var wallet = unitWork.WalletRepo.GetById(id);
            if (wallet == null)
                return NotFound("Wallet not found");
            else
            {
                unitWork.WalletRepo.Delete(id);
                return Ok("Wallet deleted successfully");
            }
        }
    }
}
