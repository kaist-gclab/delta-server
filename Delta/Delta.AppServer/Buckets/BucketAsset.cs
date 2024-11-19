using Delta.AppServer.Assets;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Buckets;

[PrimaryKey(nameof(BucketId), nameof(Path))]
public class BucketAsset
{
    public virtual required Bucket Bucket { get; set; }
    public required string Path { get; set; }
    public virtual required Asset Asset { get; set; }

    public long BucketId { get; set; }
}