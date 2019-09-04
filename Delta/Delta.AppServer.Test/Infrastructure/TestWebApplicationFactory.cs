using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Infrastructure
{
    public class TestWebApplicationFactory : WebApplicationFactory<TestStartup>
    {
        public ITestOutputHelper TestOutputHelper { get; set; }

        public HttpClient CreateAdminClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                "MY_JWT_TOKEN");
            return client;
        }

        public T GetService<T>()
        {
            CreateClient().Dispose();
            return Server.Host.Services.CreateScope().ServiceProvider.GetRequiredService<T>();
        }

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
}