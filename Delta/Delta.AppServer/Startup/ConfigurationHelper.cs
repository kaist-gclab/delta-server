using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Delta.AppServer.Startup
{
    public static class ConfigurationHelper
    {
        public static void ConfigureJsonSerializerSettings(this JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }
    }
}