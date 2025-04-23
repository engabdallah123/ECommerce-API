using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.DTO.Register
{
   public class RegisterDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Repassword { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile?  Image { get; set; }

    }
}
