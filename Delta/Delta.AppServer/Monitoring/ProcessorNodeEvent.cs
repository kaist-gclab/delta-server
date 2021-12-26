using System;
using InfluxDB.Client.Core;

namespace Delta.AppServer.Monitoring;

[Measurement("processor_node")]
public class ProcessorNodeEvent
{
    [Column(IsTimestamp = true)] public DateTime Time { get; set; }
    [Column("content")] public string Content { get; set; }
}