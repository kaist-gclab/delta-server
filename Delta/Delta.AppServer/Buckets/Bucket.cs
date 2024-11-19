using Delta.AppServer.Encryption;
using NodaTime;

namespace Delta.AppServer.Buckets;

public class Bucket
{
    public long Id { get; set; }
    public virtual required EncryptionKey? EncryptionKey { get; set; }
    public required Instant CreatedAt { get; set; }
    public virtual BucketGroup? BucketGroup { get; set; }
}