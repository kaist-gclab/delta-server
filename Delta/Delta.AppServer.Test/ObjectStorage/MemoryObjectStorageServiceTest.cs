using System;
using Delta.AppServer.ObjectStorage;
using Xunit;

namespace Delta.AppServer.Test.ObjectStorage;

public class MemoryObjectStorageServiceTest
{
    [Fact]
    public async void Write()
    {
        var service = new MemoryObjectStorageService();
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.Write("", new byte[] { }));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.Write("", new byte[] {1}));
        await service.Write("a", new byte[] { });
        await service.Write("a", new byte[] {1});
    }

    [Fact]
    public async void Read()
    {
        var service = new MemoryObjectStorageService();
        await service.Write("a", new byte[] { });
        Assert.Empty(await service.Read("a"));
        await service.Write("a", new byte[] {1});
        Assert.Single(await service.Read("a"));
        var bytes = new byte[] {3};
        await service.Write("a", bytes);
        bytes[0] = 5;
        Assert.Equal(5, bytes[0]);
        Assert.Equal(3, (await service.Read("a"))[0]);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Read(null));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await service.Read(""));
    }
}