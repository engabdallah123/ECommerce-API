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
        public async Task<List<WalletTransactionDTO>> GetTransactionByName(string name)
        {
            var transactions = await unitWork.db.WalletTransactions.Where(t => t.WalletName == name).ToListAsync();
            if (transactions == null)
                throw new Exception($"Transaction with Name {name} not found.");
            else
            {
                List<WalletTransactionDTO> transactionDTOs = new List<WalletTransactionDTO>();
                foreach (var transaction in transactions)
                {
                    WalletTransactionDTO transactionDTO = new WalletTransactionDTO()
                    {
                        Id = transaction.Id,
                        WalletId = transaction.WalletId,
                        Amount = transaction.Amount,
                        TransactionType = Enum.Parse<TransactionType>(transaction.TransactionType),
                        TransactionDate = transaction.TransactionDate,
                        Description = transaction.Description,
                        WalletName = transaction.WalletName
                    };
                    transactionDTOs.Add(transactionDTO);
                }
                
                return transactionDTOs;
            }
        }
    }
}
