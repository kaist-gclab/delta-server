using System.Linq;
using System.Threading.Tasks;
using Delta.AppServer.Assets;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.Assets;

public class CompressionServiceTest : ServiceTest
{
    public CompressionServiceTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task CompressAndDecompress()
    {
        var service = new CompressionService();
        var a = Enumerable.Range(0, 12345).Select(i => (byte)i).ToArray();
        var compressed = await service.Compress(a);
        Assert.True(a.Length > compressed.Length);
        var decompressed = await service.Decompress(compressed);
        Assert.Equal(a, decompressed);
        Assert.NotEqual(a, compressed);
        Assert.NotEqual(compressed, decompressed);
    }
}