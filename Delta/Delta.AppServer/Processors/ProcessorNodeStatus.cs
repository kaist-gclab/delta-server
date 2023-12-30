using System;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorNodeStatus
{
    public ProcessorNodeStatus(Instant timestamp, string status, ProcessorNode processorNode)
    {
        Timestamp = timestamp;
        Status = status;
        _processorNode = processorNode;
    }

    protected ProcessorNodeStatus(Instant timestamp, string status)
    {
        Timestamp = timestamp;
        Status = status;
    }

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