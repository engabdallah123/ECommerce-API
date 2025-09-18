using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Order
{
    public class OrderReadDTO
    {
       public int Id { get; set; }
        public string UserId { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderState { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderCustomerInfoDTO custInfo { get; set; }
        public List<GetOrderItemDTO> OrderItems { get; set; } = new();
    }
}
