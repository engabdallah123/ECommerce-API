using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
   public  class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }
       
        public int ProductId { get; set; }
        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product? Products { get; set; }
    }
}
