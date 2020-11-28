using System.Collections.Generic;

namespace Delta.AppServer.Processors
{
    public class RegisterProcessorNodeRequest
    {
        public string Key { get; set; }
        public IEnumerable<CreateProcessorNodeCapabilityRequest> Capabilities { get; set; }
    }
}