using System.Collections.Generic;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorVersion
    {
        public long Id { get; set; }
        public long ProcessorTypeId { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public Instant CreatedAt { get; set; }

        public ProcessorType ProcessorType { get; set; }
        public virtual ICollection<ProcessorNode> ProcessorNodes { get; set; }
        public ICollection<ProcessorVersionInputCapability> ProcessorVersionInputCapabilities { get; set; }
    }
}