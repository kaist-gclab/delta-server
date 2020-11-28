namespace Delta.AppServer.Processors
{
    public class CreateProcessorNodeCapabilityRequest
    {
        public long JobTypeId { get; set; }
        public long? AssetTypeId { get; set; }
        public string MediaType { get; set; }
    }
}