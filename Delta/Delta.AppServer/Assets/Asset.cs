using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Delta.AppServer.Jobs;
using Delta.AppServer.Encryption;
using NodaTime;

namespace Delta.AppServer.Assets
{
    public class Asset
    {
        public long Id { get; set; }
        public long AssetTypeId { get; set; }
        public long? EncryptionKeyId { get; set; }
        [Required] public string MediaType { get; set; }
        [Required] public string StoreKey { get; set; }
        public long? ParentJobExecutionId { get; set; }
        public Instant CreatedAt { get; set; }

        [Required] public virtual AssetType AssetType { get; set; }
        public virtual EncryptionKey EncryptionKey { get; set; }

        public virtual JobExecution ParentJobExecution { get; set; }
        public virtual ICollection<Job> InputJobs { get; set; }
        public virtual ICollection<AssetTag> AssetTags { get; set; } = new HashSet<AssetTag>();

        public void UpdateAssetTag(string key, string value)
        {
            var tag = (from t in AssetTags
                where t.Key == key
                select t).FirstOrDefault();
            if (tag == null)
            {
                tag = new AssetTag {Key = key};
                AssetTags.Add(tag);
            }

            tag.Value = value;
        }
    }
}