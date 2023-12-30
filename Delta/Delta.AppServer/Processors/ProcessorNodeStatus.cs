using System;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorNodeStatus
{
    public long Id { get; set; }

    private ProcessorNode? _processorNode;

    public virtual ProcessorNode ProcessorNode
    {
        get => _processorNode ?? throw new InvalidOperationException(nameof(ProcessorNode));
        set => _processorNode = value;
    }
    public required Instant Timestamp { get; set; }
    public required string Status { get; set; }
}