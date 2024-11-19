namespace Delta.AppServer.Encryption;

public record EncryptionKeyView(
    long Id,
    string Name,
    bool Enabled,
    bool Optimized,
    long BucketCount,
    long AssetCount
);