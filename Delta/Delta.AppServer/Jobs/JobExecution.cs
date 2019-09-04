namespace Delta.AppServer.Jobs
{
    public class JobExecution
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public long ProcessorNodeId { get; set; }
    }
}