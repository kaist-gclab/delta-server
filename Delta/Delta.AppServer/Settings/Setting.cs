using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Settings;

public class Setting
{
    public string Value { get; set; }
    [Key] public required string Key { get; set; }
}