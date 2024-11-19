using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobRunStatus
{
    public long Id { get; set; }
    public long JobExecutionId { get; set; }
    public Instant Timestamp { get; set; }
    public string? Status { get; set; }

    public virtual JobRun? JobExecution { get; set; }
}