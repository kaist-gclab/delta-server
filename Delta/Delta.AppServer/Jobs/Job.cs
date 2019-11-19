using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Assets;
using Delta.AppServer.Processors;
using NodaTime;

namespace Delta.AppServer.Jobs
{
    public class Job
    {
        public long Id { get; set; }
        public long ProcessorVersionId { get; set; }
        public long? InputAssetId { get; set; }
        [Required] public string JobArguments { get; set; }
        public Instant CreatedAt { get; set; }

        [Required] public virtual ProcessorVersion ProcessorVersion { get; set; }
        public virtual Asset InputAsset { get; set; }
        public virtual ICollection<JobExecution> JobExecutions { get; set; }
    }
}