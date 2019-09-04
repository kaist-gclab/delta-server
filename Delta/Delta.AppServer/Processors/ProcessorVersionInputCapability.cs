using Delta.AppServer.Assets;

namespace Delta.AppServer.Processors
{
    public class ProcessorVersionInputCapability
    {
        public long Id { get; set; }
        public long ProcessorVersionId { get; set; }
        public long AssetFormatId { get; set; }
        public long AssetTypeId { get; set; }

        public ProcessorVersion ProcessorVersion { get; set; }
        public AssetFormat AssertFormat { get; set; }
        public AssetType AssertType { get; set; }    
    }
}