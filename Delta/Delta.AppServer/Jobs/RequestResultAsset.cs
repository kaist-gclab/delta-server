using System.Collections.Generic;

namespace Delta.AppServer.Jobs;

public class RequestResultAsset
{
    public string AssetFormatKey { get; set; }
    public string AssetTypeKey { get; set; }
    public IEnumerable<RequestAssetTag> AssetTags { get; set; } = new HashSet<RequestAssetTag>();
    public byte[] Content { get; set; }
}