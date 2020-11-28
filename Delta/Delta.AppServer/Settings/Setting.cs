using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Settings
{
    public class Setting
    {
        [Key] [Required] public string Key { get; set; }
        [Required] public string Value { get; set; }
    }
}