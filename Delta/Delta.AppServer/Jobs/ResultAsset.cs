using System.Collections.Generic;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Jobs
{
    public class ResultAsset
    {
        public AssetFormat AssetFormat { get; set; }
        public AssetType AssetType { get; set; }
        public ICollection<AssetTag> AssetTags { get; set; }
        public byte[] Content { get; set; }
    }
}
