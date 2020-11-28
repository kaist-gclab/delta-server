namespace Delta.AppServer.Assets
{
    public class GetAssetResponse
    {
        public Asset Asset { get; set; }
        public string PresignedDownloadUrl { get; set; }
    }
}