using NodaTime;

namespace Delta.AppServer.Assets
{
    public class CreateModelAssetRequest
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
        public Instant EventTimestamp { get; set; }
    }
}
