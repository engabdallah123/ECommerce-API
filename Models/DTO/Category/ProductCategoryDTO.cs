using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Category
{
   public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string ?Description { get; set; }
        public decimal Price { get; set; }
        public string? Brand { get; set; }
        public double Rating { get; set; }
        public int Stock { get; set; }
        public List<string>? ImageUrl { get; set; }

        public CategoryDTO? Category { get; set; }
    }
}
