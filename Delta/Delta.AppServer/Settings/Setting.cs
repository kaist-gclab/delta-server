using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Settings;

public class Setting
{
    [Key] public required string Key { get; set; }
    public required string Value { get; set; }
}