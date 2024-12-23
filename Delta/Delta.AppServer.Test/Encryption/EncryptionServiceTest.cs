using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Encryption;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Encryption;

public class EncryptionServiceTest(ITestOutputHelper output) : ServiceTest(output)
{
    [Fact]
    public async Task BadKeyLength()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 0));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 127));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 128));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 255));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 257));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 1));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 512));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 1024));
        Assert.Empty(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 256));
        Assert.Single(context.EncryptionKey);
    }

    [Fact]
    public async Task AddEncryptionKey()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        Assert.Empty(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 256));
        Assert.Single(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("", false, false, 256));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 256));
        Assert.Single(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("B", false, false, 256));
        Assert.Equal(2, context.EncryptionKey.Count());
    }

    [Fact]
    public async Task GetEncryptionKeys()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.GetEncryptionKeys();
    }

    [Fact]
    public async Task EncryptAndDecrypt()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false, 256));
        var a = context.EncryptionKey.First();
        var data = "Delta_KqKsqvE4_테스트_WZLUI2m0_데이터"u8.ToArray();
        Assert.Null(service.Encrypt(a, data));
        a.Enabled = true;
        var encrypted = service.Encrypt(a, data);
        Assert.NotNull(encrypted);
        a.Enabled = false;
        Assert.Null(service.Decrypt(a, encrypted));
        a.Enabled = true;
        var decrypted = service.Decrypt(a, encrypted);
        Assert.NotNull(decrypted);

        Assert.NotSame(data, encrypted);
        Assert.NotSame(data, decrypted);
        Assert.NotSame(encrypted, decrypted);

        Assert.Equal(data, decrypted);
        Assert.NotEqual(data, encrypted);
        Assert.NotEqual(encrypted, decrypted);

        Assert.NotEqual(data.Length, encrypted.Length);
        Assert.Equal(data.Length, decrypted.Length);
    }

    [Fact]
    public async Task GetEncryptionKey()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a", false, false, 256));
        var a = context.EncryptionKey.OrderBy(e => e.Id).Last();
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("b", false, false, 256));
        var b = context.EncryptionKey.OrderBy(e => e.Id).Last();
        var p = await service.GetEncryptionKey(1);
        var q = await service.GetEncryptionKey(2);
        Assert.NotNull(p);
        Assert.NotNull(q);
        Assert.Null(await service.GetEncryptionKey(b.Id + 1));
        Assert.Equal(a.Id, p.Id);
        Assert.Equal(b.Id, q.Id);
    }

    [Fact]
    public async Task EnableKey()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a", false, false, 256));
        Assert.False(context.EncryptionKey.First().Enabled);
        var a = await service.GetEncryptionKey(context.EncryptionKey.First().Id);
        Assert.NotNull(a);
        Assert.False(a.Enabled);
        await service.EnableKey(a.Id);
        a = await service.GetEncryptionKey(a.Id);
        Assert.NotNull(a);
        Assert.True(a.Enabled);
    }
}