using System.Collections.Generic;

namespace Delta.AppServer.Assets
{
    public class CreateAssetRequest
    {
        public long AssetTypeId { get; set; }
        public long? EncryptionKeyId { get; set; }
        public string MediaType { get; set; }
        public string StoreKey { get; set; }
        public IEnumerable<CreateAssetTagRequest> CreateAssetTagRequest { get; set; }
        public long? ParentJobExecutionId { get; set; }
    }
}