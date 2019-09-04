using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorNodeStatus
    {
        public long Id { get; set; }
        public long ProcessorNodeId { get; set; }
        public Instant Timestamp { get; set; }
        [Required] public string Status { get; set; }

        [Required] public ProcessorNode ProcessorNode { get; set; }
    }
}