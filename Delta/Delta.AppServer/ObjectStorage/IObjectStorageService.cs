using System.Threading.Tasks;

namespace Delta.AppServer.ObjectStorage
{
    public interface IObjectStorageService
    {
        Task Write(string key, byte[] content);
        Task<byte[]> Read(string key);

        Task<string> GetPresignedUploadUrl(string key);
        Task<string> GetPresignedDownloadUrl(string key);
    }
}
