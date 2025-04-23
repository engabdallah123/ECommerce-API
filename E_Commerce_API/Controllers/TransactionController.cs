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
    [Authorize]
    public class TransactionController : ControllerBase
    {
        UnitWork unitWork;
        public TransactionController(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await unitWork.WalletTransactionRepo.GetAllAsync();
            if (transactions == null)
                return NotFound("No transactions found.");
            else
            {
                var transactionDTOs = transactions.Select(transaction => new WalletTransactionDTO()
                {
                    Id = transaction.Id,
                    WalletId = transaction.WalletId,
                    Amount = transaction.Amount,
                    TransactionType = Enum.Parse<TransactionType>(transaction.TransactionType),
                    TransactionDate = transaction.TransactionDate,
                    Description = transaction.Description,
                    WalletName= transaction.WalletName
                    
                }).ToList();

                return Ok(transactionDTOs);
            }
        }

       
        [HttpGet("Name/{name}")]
        public ActionResult GetTransactionByName(string name)
        {
            var transactions = unitWork.db.WalletTransactions.FirstOrDefault(t => t.WalletName == name);

            if (transactions == null)
                return NotFound($"Transaction with Name {name} not found.");
            else
            {
                var transactionDTOs =  new WalletTransactionDTO()
                {
                    Id = transactions.Id,
                    WalletId = transactions.WalletId,
                    Amount = transactions.Amount,
                    TransactionType = Enum.Parse<TransactionType>(transactions.TransactionType),
                    TransactionDate = transactions.TransactionDate,
                    Description = transactions.Description,
                    WalletName = transactions.WalletName
                };
                return Ok(transactionDTOs);
            }
        }
    }
}
