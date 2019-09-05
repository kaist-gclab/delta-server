using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorVersion
    {
        public long Id { get; set; }
        public long ProcessorTypeId { get; set; }
        [Required] public string Key { get; set; }
        [Required] public string Description { get; set; }
        public Instant CreatedAt { get; set; }

        [Required] public virtual ProcessorType ProcessorType { get; set; }
        public virtual ICollection<ProcessorNode> ProcessorNodes { get; set; }
        public virtual ICollection<ProcessorVersionInputCapability> ProcessorVersionInputCapabilities { get; set; }
    }
}