using System;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public class ProcessorNodeCapability
{
    public long Id { get; set; }
    public string MediaType { get; set; }

    private ProcessorNode _processorNode;

    public virtual ProcessorNode ProcessorNode
    {
        get => _processorNode ?? throw new InvalidOperationException(nameof(ProcessorNode));
        set => _processorNode = value;
    }

    private JobType _jobType;

    public virtual JobType JobType
    {
        get => _jobType ?? throw new InvalidOperationException(nameof(JobType));
        set => _jobType = value;
    }

    private AssetType? _assetType;

    public virtual AssetType? AssetType
    {
        get => _assetType;
        set => _assetType = value;
    }
}