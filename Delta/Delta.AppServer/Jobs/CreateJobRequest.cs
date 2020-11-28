namespace Delta.AppServer.Jobs
{
    public class CreateJobRequest
    {
        public long JobTypeId { get; set; }
        public long? InputAssetId { get; set; }
        public string JobArguments { get; set; }
        public long? AssignedProcessorNodeId { get; set; }
    }
}