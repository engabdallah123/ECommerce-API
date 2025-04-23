using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Review
{
   public class ReviewDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Good or Bad")]
        
        public string? Subject { get; set; }
        [Required(ErrorMessage = "Please Enter your Comment")]
        public string? Comment { get; set; }
        
        public string? UserName { get; set; }
        public string? ProductName { get; set; }
    }
}
