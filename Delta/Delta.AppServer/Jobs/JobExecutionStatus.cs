using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobExecutionStatus
{
    public long Id { get; set; }
    public long JobExecutionId { get; set; }
    public Instant Timestamp { get; set; }
    [Required] public string Status { get; set; }

    [Required] public virtual JobExecution JobExecution { get; set; }
}