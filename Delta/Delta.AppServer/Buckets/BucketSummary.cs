using NodaTime;

namespace Delta.AppServer.Buckets;

public record BucketSummary(
    long Id,
    string? EncryptionKeyName,
    Instant CreatedAt,
    string? BucketGroupName,
    string? Name,
    int TagCount
);