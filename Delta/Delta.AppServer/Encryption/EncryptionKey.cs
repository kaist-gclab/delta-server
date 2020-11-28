using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Delta.AppServer.Assets;
using Newtonsoft.Json;

namespace Delta.AppServer.Encryption
{
    public class EncryptionKey
    {
        public long Id { get; set; }
        [Required] public string Name { get; set; }
        [JsonIgnore] public string Value { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}