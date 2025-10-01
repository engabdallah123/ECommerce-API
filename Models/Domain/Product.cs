namespace Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Stock { get; set; }
        public decimal Rating { get; set; }  
        public string? Brand { get; set; }
       

        // Navigation property 
        public virtual Category? Category { get; set; } 
        

        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }


}
