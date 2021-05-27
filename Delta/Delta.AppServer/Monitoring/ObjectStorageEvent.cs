using System;
using InfluxDB.Client.Core;

namespace Delta.AppServer.Monitoring
{
    [Measurement("object_storage")]
    public class ObjectStorageEvent
    {
        [Column(IsTimestamp = true)] public DateTime Time { get; set; }
        [Column("content")] public string Content { get; set; }
    }
}