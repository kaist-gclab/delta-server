using System.Collections.Generic;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Jobs;

public class ResultAsset
{
    public required IEnumerable<AssetTag> AssetTags { get; set; }
    public required byte[] Content { get; set; }
}