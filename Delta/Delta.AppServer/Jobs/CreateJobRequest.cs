namespace Delta.AppServer.Jobs
{
    public record CreateJobRequest(
        long JobTypeId, long? InputAssetId, string JobArguments, long? AssignedProcessorNodeId);
}