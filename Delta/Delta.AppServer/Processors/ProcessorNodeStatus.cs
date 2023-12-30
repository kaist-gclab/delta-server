using System;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorNodeStatus
{
    public long Id { get; set; }
    public required Instant Timestamp { get; set; }
    public required string Status { get; set; }
    public virtual required ProcessorNode ProcessorNode { get; set; }
}