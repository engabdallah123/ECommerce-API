using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
       
        public decimal TotalPrice { get; set; }
        // Navigation properties

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        public virtual Register? User { get; set; }
    }

}
