using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Buckets;

[PrimaryKey(nameof(InputBucketId), nameof(OutputBucketId))]
public class BucketSource
{
    public virtual required Bucket InputBucket { get; set; }
    public virtual required Bucket OutputBucket { get; set; }

    public long InputBucketId { get; set; }
    public long OutputBucketId { get; set; }
}