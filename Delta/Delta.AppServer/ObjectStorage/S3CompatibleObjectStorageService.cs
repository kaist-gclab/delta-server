using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace Delta.AppServer.ObjectStorage;

public class S3CompatibleObjectStorageService : IObjectStorageService
{
    private readonly ObjectStorageConfig _objectStorageConfig;
    private readonly IObjectStorageKeyConverter _objectStorageKeyConverter;
    private readonly IMinioClient _client;

    public S3CompatibleObjectStorageService(ObjectStorageConfig objectStorageConfig,
        IObjectStorageKeyConverter objectStorageKeyConverter)
    {
        _objectStorageConfig = objectStorageConfig;
        _objectStorageKeyConverter = objectStorageKeyConverter;

        _client = new MinioClient()
            .WithEndpoint(_objectStorageConfig.Endpoint)
            .WithCredentials(_objectStorageConfig.AccessKey, _objectStorageConfig.SecretKey);
        if (_objectStorageConfig.Https)
        {
            _client = _client.WithSSL();
        }

        _client = _client.Build();
    }

    public Task<ulong> GetTotalSize()
    {
        var s = new TaskCompletionSource<ulong>();
        var objects = _client.ListObjectsAsync(
            new ListObjectsArgs().WithBucket(_objectStorageConfig.Bucket));
        ulong totalSize = 0;
        objects.Subscribe(item => { totalSize += item.Size; },
            () => { s.SetResult(totalSize); });
        return s.Task;
    }

    public async Task Write(string key, byte[] content)
    {
        await EnsureBucketExists();

        if (key == null || content == null)
        {
            throw new ArgumentNullException();
        }

        if (key == "")
        {
            throw new ArgumentOutOfRangeException();
        }

        var stream = new MemoryStream(content);
        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_objectStorageConfig.Bucket)
            .WithObject(_objectStorageKeyConverter.GetKey(key))
            .WithStreamData(stream)
            .WithObjectSize(stream.Length));
    }

    public async Task<byte[]> Read(string key)
    {
        await EnsureBucketExists();

        switch (key)
        {
            case null:
                throw new ArgumentNullException();
            case "":
                throw new ArgumentOutOfRangeException();
        }

        await using var memoryStream = new MemoryStream();
        await _client.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_objectStorageConfig.Bucket)
                .WithObject(_objectStorageKeyConverter.GetKey(key))
                .WithCallbackStream(stream => { stream.CopyTo(memoryStream); }));
        return memoryStream.ToArray();
    }

    public async Task<string> GetPresignedUploadUrl(string key)
    {
        await EnsureBucketExists();
        return await _client.PresignedPutObjectAsync(
            new PresignedPutObjectArgs()
                .WithBucket(_objectStorageConfig.Bucket)
                .WithObject(_objectStorageKeyConverter.GetKey(key))
                .WithExpiry(86400));
    }

    public async Task<string> GetPresignedDownloadUrl(string key)
    {
        await EnsureBucketExists();
        return await _client.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(_objectStorageConfig.Bucket)
                .WithObject(_objectStorageKeyConverter.GetKey(key))
                .WithExpiry(86400));
    }

    private volatile bool _initialized;
    private readonly SemaphoreSlim _initializationLock = new SemaphoreSlim(1);

    private async Task EnsureBucketExists()
    {
        if (_initialized == false)
        {
            await _initializationLock.WaitAsync();
            if (_initialized == false)
            {
                if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_objectStorageConfig.Bucket)))
                {
                    await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_objectStorageConfig.Bucket));
                }

                _initialized = true;
            }

            _initializationLock.Release();
        }
    }
}