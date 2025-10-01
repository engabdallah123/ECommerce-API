using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Cart
{
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public string userId { get; set; }
        public decimal? TotalPrice => Items.Sum(item => item.Quantity * (item.Price ??  0));

    }
}
