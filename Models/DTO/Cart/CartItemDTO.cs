using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO.Cart
{
    public class CartItemDTO
    {
        public int ProductId { get; set; }
       

        [DefaultValue(1)]
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        
    }
}
