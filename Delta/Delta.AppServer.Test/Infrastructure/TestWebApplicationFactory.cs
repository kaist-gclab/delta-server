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
            // 테스트용으로만 사용되는 토큰이므로 소스 코드에 포함하여도 안전합니다.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdXRoSW5mbyI6IntcImFjY291bnRcIjp7XCJpZFwiOjEsXCJ1c2VybmFtZVwiOlwiVEVTVF9BRE1JTl9VU0VSTkFNRVwifSxcInJvbGVcIjpcIkFkbWluXCJ9IiwianRpIjoiZjFiOWQ2OGMyMWUzMGFlZTkzZTBlZGU4NTJkNjEzZTNlNGJkNWFlMTAzZjc2NzRmYWNmMmFiY2MxYzQ4OGIwMiIsImlzcyI6IkRlbHRhLkFwcFNlcnZlci5UZXN0IiwiYXVkIjoiRGVsdGEuQXBwU2VydmVyLlRlc3QifQ.0fOQXnrtQph-QrLMMzZXx8EFJJ-frhif5Cysdfwcyog");
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