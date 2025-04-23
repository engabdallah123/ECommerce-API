using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;


namespace Models.DTO.Wallet
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Deposit=0,
        Withdrawal=1
    }
    public class WalletTransactionDTO
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public string? WalletName { get; set; } // e.g., "Main Wallet", "Savings Wallet"
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; } // e.g., "Deposit", "Withdrawal"

        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; } // e.g., "Deposit from bank", "Withdrawal for purchase"

    }
 }
  

    
