using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
   public class Register
    {
       public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Repassword { get; set; }
        public string? PhoneNumber { get; set; }

        // Navigation properties
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual Wallet? Wallet { get; set; } 
        public virtual ICollection<FolderImage>? FolderImages { get; set; }


    }
}
