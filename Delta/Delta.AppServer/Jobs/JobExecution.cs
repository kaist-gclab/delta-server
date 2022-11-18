using System;
using System.Collections.Generic;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobExecution
{
    public long Id { get; set; }
    public long JobId { get; set; }
    public long ProcessorNodeId { get; set; }

    public virtual Job Job { get; set; }
    public virtual ProcessorNode ProcessorNode { get; set; }

    public virtual ICollection<Asset> ResultAssets { get; set; } = new HashSet<Asset>();
    public virtual ICollection<JobExecutionStatus> JobExecutionStatuses { get; set; } = new HashSet<JobExecutionStatus>();

    public void AddStatus(Instant timestamp, string status)
    {
        if (status != PredefinedJobExecutionStatuses.Assigned &&
            status != PredefinedJobExecutionStatuses.Complete &&
            status != PredefinedJobExecutionStatuses.Failed)
        {
            throw new Exception();
        }

        JobExecutionStatuses.Add(new JobExecutionStatus
        {
            Timestamp = timestamp,
            Status = status
        });
    }
}