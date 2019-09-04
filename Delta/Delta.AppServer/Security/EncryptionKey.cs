using System.Collections.Generic;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Security
{
    public class EncryptionKey
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}