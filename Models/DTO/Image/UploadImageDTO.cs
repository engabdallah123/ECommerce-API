using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Image
{
    public class UploadImageDTO
    {
        public int ProductId { get; set; }

        [NotMapped]
        public List<IFormFile>? Files { get; set; }
    }
}
