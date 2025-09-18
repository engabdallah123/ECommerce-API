using Microsoft.AspNetCore.Http;
using Models.DTO.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Product
{
    public class AddProductDTO
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? stock { get; set; }
        public string? Brand { get; set; }
        public decimal? Rating { get; set; }  
        public int? CatId { get; set; }

       
    }
}
