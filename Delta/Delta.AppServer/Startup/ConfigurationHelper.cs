using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace Delta.AppServer.Startup;

public static class ConfigurationHelper
{
    public static void ConfigureJsonSerializerSettings(this JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString;
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    }
}