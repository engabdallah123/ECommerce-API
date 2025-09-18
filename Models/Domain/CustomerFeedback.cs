using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class CustomerFeedback
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty ;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // supportive / unsupportive
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public bool? Responded { get; set; } = false;
    }
}
