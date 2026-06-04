using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DTOs
{
    public class HealthStatus
    {
        public bool IsHealthy { get; set; }
        public DateTime CheckTime { get; set; }
        public Dictionary<string, ComponentHealth> Components { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
