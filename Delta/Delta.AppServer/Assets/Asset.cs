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
    }
}