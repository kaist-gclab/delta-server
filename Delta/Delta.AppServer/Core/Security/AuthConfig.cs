using System;
using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.Core.Security;

public class AuthConfig(IConfiguration configuration)
{
    public string AdminUsername => configuration["Auth:AdminUsername"] ??
                                   throw new InvalidOperationException();

    public string AdminPassword => configuration["Auth:AdminPassword"] ??
                                   throw new InvalidOperationException();
}