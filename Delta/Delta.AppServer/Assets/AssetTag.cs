namespace Delta.AppServer.Assets;

public class AssetTag
{
    public long Id { get; set; }
    public string Value { get; set; }
    public required string Key { get; set; }

    public virtual Asset Asset { get; set; }
}