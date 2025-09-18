using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Order
{
    public class GetOrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
         
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }

        public decimal Total => Quantity * UnitPrice;
    }
}
