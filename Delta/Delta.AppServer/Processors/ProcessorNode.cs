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
    public string Key { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<ProcessorNodeCapability> ProcessorNodeCapabilities { get; set; } =
        new HashSet<ProcessorNodeCapability>();

    public void UpdateCapabilities(IEnumerable<CreateProcessorNodeCapability> requests)
    {
        var removed =
            from cap in ProcessorNodeCapabilities
            where (from req in requests
                where req.MediaType == cap.MediaType &&
                      req.AssetType == cap.AssetType &&
                      req.JobType == cap.JobType
                select req).Any()
            select cap;
        foreach (var c in removed.ToList())
        {
            ProcessorNodeCapabilities.Remove(c);
        }

        var adding = from req in requests
            where (from cap in ProcessorNodeCapabilities
                where req.MediaType == cap.MediaType &&
                      req.AssetType == cap.AssetType &&
                      req.JobType == cap.JobType
                select req).Any() == false
            select req;

        foreach (var (jobType, assetType, mediaType) in adding.ToList())
        {
            ProcessorNodeCapabilities.Add(new ProcessorNodeCapability(mediaType, this, jobType, assetType));
        }
    }

    public void AddNodeStatus(Instant timestamp, string status)
    {
        if (status != PredefinedProcessorNodeStatuses.Available &&
            status != PredefinedProcessorNodeStatuses.Busy &&
            status != PredefinedProcessorNodeStatuses.Down)
        {
            throw new Exception();
        }

        ProcessorNodeStatuses.Add(new ProcessorNodeStatus(timestamp, status, this));
    }

    public virtual ICollection<JobExecution> JobExecutions { get; set; } = new HashSet<JobExecution>();

    public virtual ICollection<ProcessorNodeStatus> ProcessorNodeStatuses { get; set; } =
        new HashSet<ProcessorNodeStatus>();

    public virtual ICollection<Job> AssignedJobs { get; set; } = new HashSet<Job>();
}