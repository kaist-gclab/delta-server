using System.ComponentModel.DataAnnotations;

namespace Delta.AppServer.Settings
{
    public class Setting
    {
        public Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [Key] public string Key { get; set; }
        public string Value { get; set; }
    }
}