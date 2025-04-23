using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
   public class Order
    {
        public int Id { get; set; }
        public int RegisterId { get; set; }
       
       
        public string? RegisterName { get; set; }
        public string? RegisterEmail { get; set; }
        public string? Address { get; set; }
       public string? PhoneNumber { get; set; }             
        public decimal TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderDate { get; set; }
        public string? Status { get; set; } = "Pending";
        // Navigation properties
        [ForeignKey("RegisterId")]
        public virtual Register? Register { get; set; }
        public virtual ICollection<OrderProduct>? OrderProducts { get; set; } 

    }
}
