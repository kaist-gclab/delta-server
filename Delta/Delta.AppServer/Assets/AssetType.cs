using System.Collections.Generic;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Assets;

public class AssetType
{
    public long Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new HashSet<Asset>();
    public virtual ICollection<ProcessorNodeCapability> ProcessorNodeCapabilities { get; set; } = new HashSet<ProcessorNodeCapability>();
}