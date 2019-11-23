using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Jobs
{
    public class JobExecution
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public long ProcessorNodeId { get; set; }

        [Required] public virtual Job Job { get; set; }
        [Required] public virtual ProcessorNode ProcessorNode { get; set; }
        public virtual ICollection<Asset> ChildAssets { get; set; }

        public virtual ICollection<JobExecutionStatus> JobExecutionStatuses { get; set; } =
            new HashSet<JobExecutionStatus>();
    }
}