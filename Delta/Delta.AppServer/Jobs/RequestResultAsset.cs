using System.Collections.Generic;

namespace Delta.AppServer.Jobs;

public record RequestResultAsset(
    string AssetFormatKey,
    string AssetTypeKey,
    IEnumerable<RequestAssetTag> AssetTags,
    byte[] Content
);