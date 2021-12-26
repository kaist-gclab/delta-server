using System.Collections.Generic;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Jobs;

public class ResultAsset
{
    public AssetType AssetType { get; set; }
    public IEnumerable<AssetTag> AssetTags { get; set; }
    public byte[] Content { get; set; }
}