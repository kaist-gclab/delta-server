using System.Collections.Generic;
using System.Text.Json.Serialization;
using Delta.AppServer.Assets;

namespace Delta.AppServer.Encryption;

public class EncryptionKey
{
    public long Id { get; set; }
    public required string Name { get; set; }
    [JsonIgnore] public string? Value { get; set; }
    public required bool Enabled { get; set; }
    public required bool Optimized { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new HashSet<Asset>();
}