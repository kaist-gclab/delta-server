using System.Collections.Generic;

namespace Delta.AppServer.Processors
{
    public record RegisterProcessorNodeRequest(
        string Key, IEnumerable<CreateProcessorNodeCapabilityRequest> Capabilities);
}