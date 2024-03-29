using System;
using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.ObjectStorage;

public class ObjectStorageConfig
{
    private readonly IConfiguration _configuration;

    public ObjectStorageConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Endpoint => _configuration["ObjectStorage:Endpoint"] ??
                              throw new InvalidOperationException();

    public string AccessKey => _configuration["ObjectStorage:AccessKey"] ??
                               throw new InvalidOperationException();

    public string SecretKey => _configuration["ObjectStorage:SecretKey"] ??
                               throw new InvalidOperationException();

    public string Bucket => _configuration["ObjectStorage:Bucket"] ??
                            throw new InvalidOperationException();

    public bool Https => bool.Parse(_configuration["ObjectStorage:Https"] ??
                                    throw new InvalidOperationException());
}