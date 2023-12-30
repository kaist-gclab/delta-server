namespace Delta.AppServer.Assets;

public class AssetTag
{
    public long Id { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }

    public virtual required Asset Asset { get; set; }
}