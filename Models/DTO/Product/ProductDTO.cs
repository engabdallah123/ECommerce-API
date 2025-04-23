using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Product
{
   public class ProductDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string? Name { get; set; }


        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [DefaultValue("No description provided.")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be between 0.01 and 100,000.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Quantity in stock is required.")]
        public int stock { get; set; }
        public string? Brand { get; set; }
        public double Rating { get; set; }
       public List<string>? ImageUrl { get; set; } 
        

    }
}
