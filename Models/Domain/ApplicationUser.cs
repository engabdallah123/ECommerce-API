using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public string FullName { get; set; }
        public bool State {  get; set; } 
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
