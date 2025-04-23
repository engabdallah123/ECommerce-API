using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Cart
{
    public class CartDTO
    {
       
        public int ProductId { get; set; }
        
        public int UserId { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [DefaultValue(1)]
        public int Quantity { get; set; }
       
        public decimal? TotalPrice { get; set; }
        

    }
}
