using System;
using System.Collections.Generic;
using System.Linq;
using Delta.AppServer.Jobs;
using NodaTime;

namespace Delta.AppServer.Processors;

public class ProcessorNode
{
    public long Id { get; set; }
    public long ProcessorVersionId { get; set; }
    public required string Key { get; set; }
    public string? Name { get; set; }

    public void AddNodeStatus(Instant timestamp, string status)
    {
        if (status != PredefinedProcessorNodeStatuses.Available &&
            status != PredefinedProcessorNodeStatuses.Busy &&
            status != PredefinedProcessorNodeStatuses.Down)
        {
            throw new Exception();
        }

        ProcessorNodeStatuses.Add(new ProcessorNodeStatus
        {
            Timestamp = timestamp,
            Status = status,
            ProcessorNode = this
        });
    }

    public virtual ICollection<JobExecution> JobExecutions { get; set; } = new HashSet<JobExecution>();

    public virtual ICollection<ProcessorNodeStatus> ProcessorNodeStatuses { get; set; } =
        new HashSet<ProcessorNodeStatus>();

    public virtual ICollection<Job> AssignedJobs { get; set; } = new HashSet<Job>();
}