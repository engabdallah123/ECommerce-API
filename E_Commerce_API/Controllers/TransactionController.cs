using E_Commerce_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Wallet;
using System.Linq;
using Unit_Of_Work;

namespace E_Commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class TransactionController : ControllerBase
    { 
        private readonly TransactionService transactionService;
        public TransactionController(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet("All Transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await transactionService.GetAllTransactions();
            if (transactions == null || !transactions.Any())
                return NotFound("No transactions found.");
            else
            {
                return Ok(new { data = transactions });
            }
        }

       
        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetTransactionByName(string name)
        {
            var transaction = await transactionService.GetTransactionByName(name);
            if (transaction == null)
                return NotFound($"Transaction with Name {name} not found.");
            else
            {
                return Ok(new { data = transaction });
            }
        }
    }
}
