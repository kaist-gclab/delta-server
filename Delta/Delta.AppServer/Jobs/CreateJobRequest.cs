namespace Delta.AppServer.Jobs
{
    public class CreateJobRequest
    {
        public long InputAssetId { get; set; }
        public string JobArguments { get; set; }
        public string ProcessorVersionKey { get; set; }
    }
}