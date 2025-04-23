using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class FolderImage
    {
        public int Id { get; set; }
        public byte[]? Data { get; set; }
        public int RegisterId { get; set; }

        public virtual Register? Register { get; set; }

    }
}
