using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Assets;
using Delta.AppServer.Jobs;

namespace Delta.AppServer.Processors
{
    public class ProcessorNodeCapability
    {
        public long Id { get; set; }
        public long ProcessorNodeId { get; set; }
        public long JobTypeId { get; set; }
        public long? AssetTypeId { get; set; }
        [Required] public string MediaType { get; set; }

        [Required] public virtual ProcessorNode ProcessorNode { get; set; }
        [Required] public virtual JobType JobType { get; set; }
        public virtual AssetType AssetType { get; set; }
    }
}