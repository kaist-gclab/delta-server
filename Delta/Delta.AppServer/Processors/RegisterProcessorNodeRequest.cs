using System.Collections.Generic;

namespace Delta.AppServer.Processors
{
    public class RegisterProcessorNodeRequest
    {
        public string ProcessorTypeKey { get; set; }
        public string ProcessorVersionKey { get; set; }
        public string ProcessorVersionDescription { get; set; }
        public string ProcessorNodeKey { get; set; }
        public string ProcessorNodeName { get; set; }

        public IEnumerable<InputCapability> InputCapabilities { get; set; }
        
        public class InputCapability
        {
            public string AssetFormatKey { get; set; }
            public string AssetTypeKey { get; set; }
        }
    }
}