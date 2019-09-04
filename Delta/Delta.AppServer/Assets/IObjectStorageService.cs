using System;
using System.Threading.Tasks;

namespace Delta.AppServer.Assets
{
    public interface IObjectStorageService
    {
        Task Write(string key, byte[] content);
        Task<byte[]> Read(string key);
    }
}