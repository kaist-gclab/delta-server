using System;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorNodeStatus
{
    public long Id { get; set; }
    public Instant Timestamp { get; set; }
    public string Status { get; set; }

    private ProcessorNode? _processorNode;

    public virtual ProcessorNode ProcessorNode
    {
        get => _processorNode ?? throw new InvalidOperationException(nameof(ProcessorNode));
        set => _processorNode = value;
    }
}