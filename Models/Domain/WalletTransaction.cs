using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
   public class WalletTransaction
    {
        public int Id { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }           
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now.Date;
        public int WalletId { get; set; }
        public string? WalletName { get; set; }
        public virtual Wallet? Wallet { get; set; } 

    }
}
