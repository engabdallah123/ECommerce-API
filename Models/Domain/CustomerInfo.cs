using Models.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        public string CustName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerInfo() { }

        public CustomerInfo(OrderCustomerInfoDTO dto)
        {
            CustName = dto.Name;
            Address = dto.Address;
            Email = dto.Email;
            Phone = dto.Phone;
        }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
