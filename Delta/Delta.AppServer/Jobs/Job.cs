using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class Job
    {
        public long Id { get; set; }
        public long ProcessorTypeId { get; set; }
        public long InputAssetId { get; set; }
        public string JobArguments { get; set; }
        public Instant CreatedAt { get; set; }
    }
}