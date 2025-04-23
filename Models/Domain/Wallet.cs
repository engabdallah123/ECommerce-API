using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Wallet
    {
        public int Id { get; set; }
        public int? UserId { get; set; } // = 2 for system wallet
        public decimal? Balance { get; set; }
        public string? UserName { get; set; }

        public virtual Register? Register { get; set; }
        public virtual ICollection<WalletTransaction>? Transactions { get; set; } 
    }
}
