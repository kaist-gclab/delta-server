using System.Collections.Generic;
using NodaTime;

namespace Delta.AppServer.Buckets;

public record BucketView(
    long Id,
    string? EncryptionKeyName,
    Instant CreatedAt,
    string? BucketGroupName,
    string? Name,
    IEnumerable<BucketTagView> Tags
);