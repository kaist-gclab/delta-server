using System.Collections.Generic;
using System.Linq;
using Delta.AppServer.Jobs;
using Delta.AppServer.Encryption;
using NodaTime;

namespace Delta.AppServer.Assets;

public class Asset
{
    public long Id { get; set; }
    public long? EncryptionKeyId { get; set; }
    public required string MediaType { get; set; }
    public required string StoreKey { get; set; }
    public required Instant CreatedAt { get; set; }

    public virtual required AssetType AssetType { get; set; }

    public virtual EncryptionKey? EncryptionKey { get; set; }

    public virtual required JobExecution? ParentJobExecution { get; set; }
    public virtual ICollection<Job> InputJobs { get; set; } = new HashSet<Job>();
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