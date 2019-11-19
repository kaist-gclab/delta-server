using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Processors
{
    public class ProcessorVersionInputCapability
    {
        public long Id { get; set; }
        public long ProcessorVersionId { get; set; }
        public long? AssetFormatId { get; set; }
        public long? AssetTypeId { get; set; }

        [Required] public virtual ProcessorVersion ProcessorVersion { get; set; }
        public virtual AssetFormat AssertFormat { get; set; }
        public virtual AssetType AssertType { get; set; }
    }
}