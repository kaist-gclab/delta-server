using Delta.AppServer.Encryption;
using NodaTime;

namespace Delta.AppServer.Assets;

public class Bucket
{
    public long Id { get; set; }
    public required Instant Created { get; set; }
    public virtual required EncryptionKey? EncryptionKey { get; set; }
}