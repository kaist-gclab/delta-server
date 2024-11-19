using System.Collections.Generic;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Jobs;

public class JobType
{
    public long Id { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
}