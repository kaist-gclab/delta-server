using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobExecutionStatus
{
    public long Id { get; set; }
    public long JobExecutionId { get; set; }
    public Instant Timestamp { get; set; }
    public string Status { get; set; }

    public virtual JobExecution JobExecution { get; set; }
}