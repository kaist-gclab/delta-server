using System;
using System.Collections.Generic;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs;

public class JobRun
{
    public long Id { get; set; }

    public virtual required Job Job { get; set; }
    public virtual required ProcessorNode ProcessorNode { get; set; }

    public virtual ICollection<Asset> ResultAssets { get; set; } = new HashSet<Asset>();
    public virtual ICollection<JobRunStatus> JobExecutionStatuses { get; set; } = new HashSet<JobRunStatus>();

    public void AddStatus(Instant timestamp, string status)
    {
        if (status != PredefinedJobExecutionStatuses.Assigned &&
            status != PredefinedJobExecutionStatuses.Complete &&
            status != PredefinedJobExecutionStatuses.Failed)
        {
            throw new Exception();
        }

        JobExecutionStatuses.Add(new JobRunStatus
        {
            Timestamp = timestamp,
            Status = status
        });
    }
}