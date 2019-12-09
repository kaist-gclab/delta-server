namespace Delta.AppServer.Assets
{
    public class CreateAssetRequest
    {
        public string AssetFormatKey { get; set; }
        public string AssetTypeKey { get; set; }
        public byte[] Content { get; set; }
        public string EncryptionKeyName { get; set; }
    }
}