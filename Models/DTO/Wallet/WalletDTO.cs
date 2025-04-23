using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Wallet
{
   public class WalletDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal? Balance { get; set; }
        public string? UserName { get; set; }
    }
}
