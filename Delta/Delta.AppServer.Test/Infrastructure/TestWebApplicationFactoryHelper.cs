using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Delta.AppServer.Test.Infrastructure;

public static class TestWebApplicationFactoryHelper
{
    public static HttpClient CreateAdminClient<T>(this WebApplicationFactory<T> webApplicationFactory)
        where T : class
    {
        var client = webApplicationFactory.CreateClient();
        // 테스트용으로만 사용되는 토큰이므로 소스 코드에 포함하여도 안전합니다.
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdXRoSW5mbyI6IntcImFjY291bnRcIjp7XCJpZFwiOjEsXCJ1c2VybmFtZVwiOlwiVEVTVF9BRE1JTl9VU0VSTkFNRVwifSxcInJvbGVcIjpcIkFkbWluXCJ9IiwianRpIjoiZjFiOWQ2OGMyMWUzMGFlZTkzZTBlZGU4NTJkNjEzZTNlNGJkNWFlMTAzZjc2NzRmYWNmMmFiY2MxYzQ4OGIwMiIsImlzcyI6IkRlbHRhLkFwcFNlcnZlci5UZXN0IiwiYXVkIjoiRGVsdGEuQXBwU2VydmVyLlRlc3QifQ.sB-bZ1BrpbuZgIlGs-AsxQ9vYmy21ktr6f1-gK6Z2_Q");
        return client;
    }

    public static ServiceProvider<T> GetServiceProvider<T>(this WebApplicationFactory<T> webApplicationFactory)
        where T : class
    {
        return new ServiceProvider<T>(webApplicationFactory);
    }
}

public class ServiceProvider<T> where T : class
{
    private readonly WebApplicationFactory<T> _webApplicationFactory;

    public ServiceProvider(WebApplicationFactory<T> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    public TService Get<TService>() where TService : notnull
    {
        _webApplicationFactory.CreateClient().Dispose();
        return _webApplicationFactory.Server.Host.Services.CreateScope().ServiceProvider
            .GetRequiredService<TService>();
    }
}