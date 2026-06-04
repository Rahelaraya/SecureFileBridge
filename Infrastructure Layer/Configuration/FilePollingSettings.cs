using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Configuration
{
    public class FilePollingSettings
    {
        public string SourcePath { get; set; } = string.Empty;

        public int PollingIntervalSeconds { get; set; }

        public int MaxRetryAttempts { get; set; }

        public string FilePattern { get; set; } = "*.*";

        public NetworkCredentialSettings NetworkCredential { get; set; }
            = new();
    }

}
