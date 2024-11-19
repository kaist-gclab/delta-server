using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Encryption;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Encryption;

public class EncryptionServiceTest : ServiceTest
{
    public EncryptionServiceTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddEncryptionKey()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        Assert.Empty(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false));
        Assert.Single(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("", false, false));
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false));
        Assert.Single(context.EncryptionKey);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("B", false, false));
        Assert.Equal(2, context.EncryptionKey.Count());
    }

    [Fact]
    public void GetEncryptionKeys()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        service.GetEncryptionKeys();
    }

    [Fact]
    public async Task EncryptAndDecrypt()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A", false, false));
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
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a", false, false));
        var a = context.EncryptionKey.OrderBy(e => e.Id).Last();
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("b", false, false));
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
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a", false, false));
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