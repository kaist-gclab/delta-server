using System;
using System.IO;
using System.Threading;
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
            EnsureBucketExists();

            if (key == null || content == null)
            {
                throw new ArgumentNullException();
            }

            if (key == "")
            {
                throw new ArgumentOutOfRangeException();
            }

            var stream = new MemoryStream(content);
            await _client.PutObjectAsync(_objectStorageConfig.Bucket, key, stream, stream.Length);
        }

        public async Task<byte[]> Read(string key)
        {
            EnsureBucketExists();

            if (key == null)
            {
                throw new ArgumentNullException();
            }

            if (key == "")
            {
                throw new ArgumentOutOfRangeException();
            }

            await using var memoryStream = new MemoryStream();
            await _client.GetObjectAsync(_objectStorageConfig.Bucket, key,
                stream => { stream.CopyTo(memoryStream); });
            return memoryStream.ToArray();
        }

        private volatile bool _initialized;
        private readonly SemaphoreSlim _initializationLock = new SemaphoreSlim(1);

        private async void EnsureBucketExists()
        {
            if (_initialized == false)
            {
                await _initializationLock.WaitAsync();
                if (_initialized == false)
                {
                    if (!await _client.BucketExistsAsync(_objectStorageConfig.Bucket))
                    {
                        await _client.MakeBucketAsync(_objectStorageConfig.Bucket);
                    }

                    _initialized = true;
                }

                _initializationLock.Release();
            }
        }
    }
}