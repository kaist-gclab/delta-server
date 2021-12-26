using System;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public class ProcessorNodeCapability
{
    public ProcessorNodeCapability(
        string mediaType, ProcessorNode processorNode, JobType jobType, AssetType? assetType)
    {
        MediaType = mediaType;
        _processorNode = processorNode;
        _jobType = jobType;
        _assetType = assetType;
    }

    protected ProcessorNodeCapability(string mediaType)
    {
        MediaType = mediaType;
    }

    public long Id { get; set; }
    public long ProcessorNodeId { get; set; }
    public long JobTypeId { get; set; }
    public long? AssetTypeId { get; set; }
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