using System.Collections.Generic;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Jobs;

public class JobType
{
    public long Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    public virtual ICollection<ProcessorNodeCapability> ProcessorNodeCapabilities { get; set; } = new HashSet<ProcessorNodeCapability>();
}