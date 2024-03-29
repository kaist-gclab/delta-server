using System;
using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.Core.Security;

public class AuthConfig
{
    private readonly IConfiguration _configuration;

    public AuthConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AdminUsername => _configuration["Auth:AdminUsername"] ??
                                   throw new InvalidOperationException();

    public string AdminPassword => _configuration["Auth:AdminPassword"] ??
                                   throw new InvalidOperationException();
}