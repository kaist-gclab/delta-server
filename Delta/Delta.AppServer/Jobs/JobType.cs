using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Jobs
{
    public class JobType
    {
        public long Id { get; set; }
        [Required] public string Key { get; set; }
        [Required] public string Name { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<ProcessorNodeCapability> ProcessorNodeCapabilities { get; set; }
    }
}