using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors
{
    public class ProcessorType
    {
        public long Id { get; set; }
        [Required] public string Key { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProcessorVersion> ProcessorVersions { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}