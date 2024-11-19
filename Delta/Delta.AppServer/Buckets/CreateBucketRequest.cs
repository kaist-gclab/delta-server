namespace Delta.AppServer.Buckets;

public record CreateBucketRequest(string EncryptionKeyName, string Name);
