using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DTOs
{
    public class FlowStatusResponse
    {
        public string Message { get; set; } = string.Empty;

        public string PollingStatus { get; set; } = string.Empty;

        public int SourceFilesWaiting { get; set; }

        public int ProcessedFiles { get; set; }

        public string RabbitMqStatus { get; set; } = string.Empty;

        public string? LatestProcessedFile { get; set; }

        public DateTime? LatestProcessedAt { get; set; }
    }
}
