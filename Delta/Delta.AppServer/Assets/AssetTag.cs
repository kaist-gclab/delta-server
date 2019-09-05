using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Assets
{
    public class AssetTag
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        [Required] public string Key { get; set; }
        [Required] public string Value { get; set; }

        [Required] public virtual Asset Asset { get; set; }
    }
}