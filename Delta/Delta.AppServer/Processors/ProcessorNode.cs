using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors
{
    public class ProcessorNode
    {
        public long Id { get; set; }
        public long ProcessorVersionId { get; set; }
        [Required] public string Key { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProcessorNodeCapability> ProcessorNodeCapabilities { get; set; } =
            new HashSet<ProcessorNodeCapability>();

        public virtual ICollection<JobExecution> JobExecutions { get; set; }

        public virtual ICollection<ProcessorNodeStatus> ProcessorNodeStatuses { get; set; } =
            new HashSet<ProcessorNodeStatus>();

        public virtual ICollection<Job> AssignedJobs { get; set; }
    }
}