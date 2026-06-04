using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Interfaces
{
    public interface IHeartbeatTelemetryService
    {
        void EmitHeartbeat(
            int heartbeatCount,
            long memoryUsage);
    }
}
