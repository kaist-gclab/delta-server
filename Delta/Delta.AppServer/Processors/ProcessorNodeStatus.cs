using NodaTime;

namespace Delta.AppServer.Processors
{
    public class ProcessorNodeStatus
    {
        public long Id { get; set; }
        public long ProcessorNodeId { get; set; }
        public Instant Timestamp { get; set; }
        public string Status { get; set; }

        public ProcessorNode ProcessorNode { get; set; }
    }
}