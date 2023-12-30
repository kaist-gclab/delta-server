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
        Assert.Empty(context.EncryptionKeys);
        await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A"));
        Assert.Single(context.EncryptionKeys);
        Assert.Null(await service.AddEncryptionKey(new CreateEncryptionKeyRequest("")));
        Assert.Null(await service.AddEncryptionKey(new CreateEncryptionKeyRequest("A")));
        Assert.Single(context.EncryptionKeys);
        Assert.NotNull(await service.AddEncryptionKey(new CreateEncryptionKeyRequest("B")));
        Assert.Equal(2, context.EncryptionKeys.Count());
    }

    [Fact]
    public void GetEncryptionKeys()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        service.GetEncryptionKeys();
    }

    [Fact]
    public void EncryptAndDecrypt()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        service.AddEncryptionKey("A");
        var a = service.GetEncryptionKeys().First();
        var data = Encoding.UTF8.GetBytes("Delta_KqKsqvE4_테스트_WZLUI2m0_데이터");
        Assert.Throws<Exception>(() => { service.Encrypt(a, data); });
        a.Enabled = true;
        var encrypted = service.Encrypt(a, data);
        a.Enabled = false;
        Assert.Throws<Exception>(() => { service.Decrypt(a, encrypted); });
        a.Enabled = true;
        var decrypted = service.Decrypt(a, encrypted);

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
        var a = await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a"));
        var b = await service.AddEncryptionKey(new CreateEncryptionKeyRequest("b"));
        var p = await service.GetEncryptionKey("a");
        var q = await service.GetEncryptionKey("b");
        Assert.NotNull(a);
        Assert.NotNull(b);
        Assert.NotNull(p);
        Assert.NotNull(q);
        Assert.Null(await service.GetEncryptionKey("c"));
        Assert.Equal(a.EncryptionKey.Id, p.Id);
        Assert.Equal(b.EncryptionKey.Id, q.Id);
    }

    [Fact]
    public async Task EnableKey()
    {
        var context = CreateDbContext();
        var service = new EncryptionService(context);
        var response = await service.AddEncryptionKey(new CreateEncryptionKeyRequest("a"));
        Assert.NotNull(response);
        Assert.False(response.EncryptionKey.Enabled);
        var a = await service.GetEncryptionKey("a");
        Assert.NotNull(a);
        Assert.False(a.Enabled);
        await service.EnableKey(a.Id);
        a = await service.GetEncryptionKey("a");
        Assert.NotNull(a);
        Assert.True(a.Enabled);
    }
}