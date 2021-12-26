using System.Collections.Generic;

namespace Delta.AppServer.Assets;

public record CreateAssetRequest(
    long AssetTypeId, long? EncryptionKeyId, string MediaType, string StoreKey,
    IEnumerable<CreateAssetTagRequest> CreateAssetTagRequest,
    long? ParentJobExecutionId);