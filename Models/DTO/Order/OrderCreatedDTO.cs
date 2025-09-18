using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Order
{
   
    public class OrderCreatedDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }  
        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItemCreateDTO> Items { get; set; } = new();     
        public OrderCustomerInfoDTO custInfo { get; set; }
    }
}
