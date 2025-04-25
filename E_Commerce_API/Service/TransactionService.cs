using Microsoft.EntityFrameworkCore;
using Models.DTO.Wallet;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class TransactionService
    {
        UnitWork unitWork;
        public TransactionService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public async Task<List<WalletTransactionDTO>> GetAllTransactions()
        {
            var transactions = await unitWork.WalletTransactionRepo.GetAllAsync();
            if (transactions == null)
                throw new Exception("No transactions found.");
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
                    WalletName = transaction.WalletName

                }).ToList();
                return transactionDTOs;
            }
        }
        public async Task<WalletTransactionDTO> GetTransactionByName(string name)
        {
            var transactions = await unitWork.db.WalletTransactions.FirstOrDefaultAsync(t => t.WalletName == name);
            if (transactions == null)
                throw new Exception($"Transaction with Name {name} not found.");
            else
            {
                var transactionDTOs = new WalletTransactionDTO()
                {
                    Id = transactions.Id,
                    WalletId = transactions.WalletId,
                    Amount = transactions.Amount,
                    TransactionType = Enum.Parse<TransactionType>(transactions.TransactionType),
                    TransactionDate = transactions.TransactionDate,
                    Description = transactions.Description,
                    WalletName = transactions.WalletName
                };
                return transactionDTOs;
            }
        }
    }
}
