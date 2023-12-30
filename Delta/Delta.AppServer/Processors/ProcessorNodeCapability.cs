using System;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors;

public class ProcessorNodeCapability
{
    public long Id { get; set; }
    public string MediaType { get; set; }
    public virtual ProcessorNode ProcessorNode { get; set; }
    public virtual JobType JobType { get; set; }
    public virtual AssetType? AssetType { get; set; }
}