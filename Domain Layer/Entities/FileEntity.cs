using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
   
    public class FileEntity
    {
        public string FileName { get; set; } 

        public string FilePath { get; set; } 

        public long FileSize { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedAt { get; set; }

        public string? ErrorMessage { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
