using System;
using System.Linq;
using System.Text;
using Delta.AppServer.Security;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Security
{
    public class EncryptionServiceTest : ServiceTest
    {
        public EncryptionServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEncryptionKey()
        {
            var context = CreateDbContext();
            var service = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            Assert.Empty(context.EncryptionKeys);
            service.AddEncryptionKey("A");
            Assert.Single(context.EncryptionKeys);
            Assert.Throws<Exception>(() => service.AddEncryptionKey(null));
            Assert.Throws<Exception>(() => service.AddEncryptionKey(""));
            Assert.Throws<Exception>(() => service.AddEncryptionKey("A"));
            Assert.Single(context.EncryptionKeys);
            service.AddEncryptionKey("B");
            Assert.Equal(2, context.EncryptionKeys.Count());
        }

        [Fact]
        public void GetEncryptionKeys()
        {
            var context = CreateDbContext();
            var service = new EncryptionService(context, Output.ToLogger<EncryptionService>());
            service.GetEncryptionKeys();
        }

        [Fact]
        public void EncryptAndDecrypt()
        {
            var context = CreateDbContext();
            var service = new EncryptionService(context, Output.ToLogger<EncryptionService>());
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
    }
}