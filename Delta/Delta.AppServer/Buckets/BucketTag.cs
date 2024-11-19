namespace Delta.AppServer.Buckets;

public class BucketTag
{
    public long Id { get; set; }
    public virtual required Bucket Bucket { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
}