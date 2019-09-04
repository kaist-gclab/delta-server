using System.Collections.Generic;
using Delta.AppServer.Jobs;
using Delta.AppServer.Security;
using NodaTime;

namespace Delta.AppServer.Assets
{
    public class Asset
    {
        public long Id { get; set; }
        public long AssetFormatId { get; set; }
        public long? EncryptionKeyId { get; set; }
        public long AssetTypeId { get; set; }
        public string StoreKey { get; set; }
        public long ParentJobExecutionId { get; set; }
        public Instant CreatedAt { get; set; }

        public AssetFormat AssetFormat { get; set; }
        public AssetType AssetType { get; set; }
        public JobExecution ParentJobExecution { get; set; }
        public EncryptionKey EncryptionKey { get; set; }
        public virtual ICollection<Job> InputJobs { get; set; }
        public ICollection<AssetTag> AssetTags { get; set; }
    }
}