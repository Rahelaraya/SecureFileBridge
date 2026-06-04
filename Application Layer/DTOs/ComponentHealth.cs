using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DTOs
{
    public class ComponentHealth
    {
        public bool IsHealthy { get; set; } = true;
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public string? LastError { get; set; }
        public DateTime LastCheckTime { get; set; } = DateTime.UtcNow;
    }
}
