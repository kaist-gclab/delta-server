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
        public long ProcessorTypeId { get; set; }
        public long? InputAssetId { get; set; }
        [Required] public string JobArguments { get; set; }
        public Instant CreatedAt { get; set; }

        [Required] public ProcessorType ProcessorType { get; set; }
        public Asset InputAsset { get; set; }
        public ICollection<JobExecution> JobExecutions { get; set; }
    }
}