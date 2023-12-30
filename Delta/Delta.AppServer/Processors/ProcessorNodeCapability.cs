using System;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public class ProcessorNodeCapability
{
    public long Id { get; set; }
    public virtual AssetType? AssetType { get; set; }
    public required string MediaType { get; set; }
    public virtual required ProcessorNode ProcessorNode { get; set; }
    public virtual required JobType JobType { get; set; }
}