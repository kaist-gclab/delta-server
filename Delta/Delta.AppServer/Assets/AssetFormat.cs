using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Processors;

namespace Delta.AppServer.Assets
{
    public class AssetFormat
    {
        public long Id { get; set; }
        [Required] public string Key { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        public string FileExtension { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<ProcessorVersionInputCapability> ProcessorVersionInputCapabilities { get; set; }
    }
}