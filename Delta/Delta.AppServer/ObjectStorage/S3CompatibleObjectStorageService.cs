using System.IO;
using System.Threading.Tasks;
using Minio;

namespace Delta.AppServer.ObjectStorage
{
    public class S3CompatibleObjectStorageService : IObjectStorageService
    {
        private readonly ObjectStorageConfig _objectStorageConfig;
        private readonly MinioClient _client;

        public S3CompatibleObjectStorageService(ObjectStorageConfig objectStorageConfig)
        {
            _objectStorageConfig = objectStorageConfig;
            _client = new MinioClient(_objectStorageConfig.Endpoint,
                _objectStorageConfig.AccessKey,
                _objectStorageConfig.SecretKey);
        }

        public async Task Write(string key, byte[] content)
        {
            var stream = new MemoryStream(content);
            await _client.PutObjectAsync(_objectStorageConfig.Bucket, key, stream, stream.Length);
        }

        public async Task<byte[]> Read(string key)
        {
            using (var memoryStream = new MemoryStream())
            {
                await _client.GetObjectAsync(_objectStorageConfig.Bucket, key,
                    stream => { stream.CopyTo(memoryStream); });
                return memoryStream.ToArray();
            }
        }
    }
}