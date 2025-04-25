using Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Cart
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [DefaultValue(1)]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalPrice { get; set; }      
        
    }
}
