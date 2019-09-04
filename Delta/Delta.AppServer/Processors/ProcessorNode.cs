using System.Collections.Generic;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors
{
    public class ProcessorNode
    {
        public long Id { get; set; }
        public long ProcessorVersionId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public ProcessorVersion ProcessorVersion { get; set; }
        public virtual ICollection<JobExecution> JobExecutions { get; set; }
        public ICollection<ProcessorNodeStatus> ProcessorNodeStatuses { get; set; }
    }
}