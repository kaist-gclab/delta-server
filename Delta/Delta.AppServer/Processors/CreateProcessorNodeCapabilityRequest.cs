namespace Delta.AppServer.Processors
{
    public record CreateProcessorNodeCapabilityRequest(long JobTypeId, long? AssetTypeId, string MediaType);
}