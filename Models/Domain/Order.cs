using Microsoft.AspNetCore.Identity;
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
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; }   
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public int CustId { get; set; }
        public virtual CustomerInfo CustomerInfo { get; set; }


        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
