using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure;

public class TestWebApplicationFactory : WebApplicationFactory<TestStartup>
{
    public ITestOutputHelper TestOutputHelper { get; set; }
        
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("");
        builder.UseEnvironment("Test");
        builder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.Add(new JsonConfigurationSource {Path = "Delta.AppServer/appsettings.json"});
            configurationBuilder.Add(new JsonConfigurationSource {Path = "Delta.AppServer.Test/appsettings.Test.json"});
        });
        builder.ConfigureServices((context, collection) =>
        {
            collection.AddSingleton<ITestOutputHelper>(new TestOutputHelperProxy(this));
        });
    }

    protected override IWebHostBuilder CreateWebHostBuilder() =>
        WebHost.CreateDefaultBuilder().UseStartup<TestStartup>();
}