using System.Collections.Generic;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class Job
{
    public long Id { get; set; }
    public required string JobArguments { get; set; }
    public required Instant CreatedAt { get; set; }
    public long? AssignedProcessorNodeId { get; set; }

    public virtual required JobType JobType { get; set; }
    public virtual required Asset? InputAsset { get; set; }
    public virtual ICollection<JobRun> JobExecutions { get; set; } = new HashSet<JobRun>();
    public virtual ProcessorNode? AssignedProcessorNode { get; set; }
}