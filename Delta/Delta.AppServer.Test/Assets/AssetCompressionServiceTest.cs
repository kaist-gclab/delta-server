using System.Linq;
using Delta.AppServer.Assets;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Assets
{
    public class AssetCompressionServiceTest: ServiceTest
    {
        public AssetCompressionServiceTest(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void CompressAndDecompress()
        {
            var service = new AssetCompressionService();
            var a = Enumerable.Range(0, 12345).Select(i => (byte) i).ToArray();
            var compressed = service.Compress(a);
            Assert.True(a.Length > compressed.Length);
            var decompressed = service.Decompress(compressed);
            Assert.Equal(a, decompressed);
            Assert.NotEqual(a, compressed);
            Assert.NotEqual(compressed, decompressed);
        }
    }
}