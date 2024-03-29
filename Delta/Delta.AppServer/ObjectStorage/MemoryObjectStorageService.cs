using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delta.AppServer.ObjectStorage;

public class MemoryObjectStorageService : IObjectStorageService
{
    public Dictionary<string, byte[]> Storage { get; } = new();

    public Task Write(string key, byte[] content)
    {
        if (key == "")
        {
            throw new ArgumentOutOfRangeException();
        }

        Storage[key] = (byte[]) content.Clone();
        return Task.CompletedTask;
    }

    public Task<byte[]> Read(string key)
    {
        if (key == "")
        {
            throw new ArgumentOutOfRangeException();
        }

        return Task.FromResult((byte[]) Storage[key].Clone());
    }

    public Task<string> GetPresignedUploadUrl(string key)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetPresignedDownloadUrl(string key)
    {
        throw new NotImplementedException();
    }

    public Task<ulong> GetTotalSize()
    {
        throw new NotImplementedException();
    }
}