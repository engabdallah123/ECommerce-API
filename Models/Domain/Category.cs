using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
  public  class Category
  {
     

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        // navagation property
        public virtual ICollection<Product>? Products { get; set; } 



    }


}
