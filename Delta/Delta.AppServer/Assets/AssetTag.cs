namespace Delta.AppServer.Assets;

public class AssetTag
{
    public long Id { get; set; }
    public long AssetId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    public virtual Asset Asset { get; set; }
}