using System;
using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.ObjectStorage;

public class ObjectStorageConfig(IConfiguration configuration)
{
    public string Endpoint => configuration["ObjectStorage:Endpoint"] ??
                              throw new InvalidOperationException();

    public string AccessKey => configuration["ObjectStorage:AccessKey"] ??
                               throw new InvalidOperationException();

    public string SecretKey => configuration["ObjectStorage:SecretKey"] ??
                               throw new InvalidOperationException();

    public string Bucket => configuration["ObjectStorage:Bucket"] ??
                            throw new InvalidOperationException();

    public bool Https => bool.Parse(configuration["ObjectStorage:Https"] ??
                                    throw new InvalidOperationException());
}