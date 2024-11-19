using System.Collections.Generic;
using System.Text.Json.Serialization;
using Delta.AppServer.Buckets;
using Microsoft.EntityFrameworkCore;

namespace Delta.AppServer.Encryption;

[Index(nameof(Name), IsUnique = true)]
public class EncryptionKey
{
    public long Id { get; set; }
    public required string Name { get; set; }
    [JsonIgnore] public string? Value { get; set; }
    public required bool Enabled { get; set; }
    public required bool Optimized { get; set; }

    public virtual ICollection<Bucket> Buckets { get; set; } = new HashSet<Bucket>();
}