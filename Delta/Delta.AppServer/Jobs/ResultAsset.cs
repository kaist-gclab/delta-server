using System.Collections.Generic;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Jobs;

public class ResultAsset
{
    public byte[] Content { get; set; }
    public required AssetType AssetType { get; set; }
    public required IEnumerable<AssetTag> AssetTags { get; set; }
}