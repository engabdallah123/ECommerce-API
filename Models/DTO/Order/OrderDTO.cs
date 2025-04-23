using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Order
{
   
    public class OrderDTO
    {
        public int Id { get; set; }
        public int RegisterId { get; set; }
        public string? RegisterName { get; set; }
        public string? RegisterEmail { get; set; }
        public string? RegisterPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter your Address")]
        public string? Address { get; set; }       
        public decimal? TotalPrice { get; set; }

        [DefaultValue("CashOnDelivery")]
        public string? PaymentMethod { get; set; }

        [DefaultValue("Pending")]
        public string? Status { get; set; }
        
        public string? OrderDate { get; set; } 
    }
}
