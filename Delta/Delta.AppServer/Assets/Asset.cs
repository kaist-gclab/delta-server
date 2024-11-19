using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace Delta.AppServer.Assets;

public class Asset
{
    public long Id { get; set; }
    public required string StoreKey { get; set; }
    public required Instant CreatedAt { get; set; }
    public virtual ICollection<AssetTag> AssetTags { get; set; } = new HashSet<AssetTag>();

    public void UpdateAssetTag(string key, string value)
    {
        var tag = (from t in AssetTags
            where t.Key == key
            select t).FirstOrDefault();

        if (tag == null)
        {
            tag = new AssetTag
            {
                Key = key,
                Value = value,
                Asset = this
            };
            AssetTags.Add(tag);
        }
        else
        {
            tag.Value = value;
        }
    }
}