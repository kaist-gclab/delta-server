using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Settings;

public class Setting
{
    [Key] public string Key { get; set; }
    public string Value { get; set; }
}