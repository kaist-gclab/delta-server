namespace Delta.AppServer.Buckets;

public class BucketGroup
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required int Order { get; set; }
}