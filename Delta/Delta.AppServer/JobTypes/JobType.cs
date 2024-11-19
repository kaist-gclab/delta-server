using System.Collections.Generic;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.JobTypes;

public class JobType
{
    public long Id { get; set; }
    public required string Key { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
}