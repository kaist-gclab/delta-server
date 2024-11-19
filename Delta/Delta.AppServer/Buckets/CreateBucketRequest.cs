namespace Delta.AppServer.Buckets;

public record CreateBucketRequest(long EncryptionKeyId, string Name);
