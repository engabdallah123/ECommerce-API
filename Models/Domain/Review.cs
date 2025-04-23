using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
   public  class Review
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Product? Products { get; set; }
        public virtual Register? Users { get; set; }

    }
}
