using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Test.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Delta.AppServer.Test.ObjectStorage;

public class PrefixFourObjectStorageKeyConverterTest : ServiceTest
{
    public PrefixFourObjectStorageKeyConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetKey()
    {
        var service = new PrefixFourObjectStorageKeyConverter();
        Assert.Equal("$/$/$/$/", service.GetKey(""));
        Assert.Equal("1/$/$/$/1", service.GetKey("1"));
        Assert.Equal("1/2/$/$/12", service.GetKey("12"));
        Assert.Equal("1/2/3/$/123", service.GetKey("123"));
        Assert.Equal("1/2/3/4/1234", service.GetKey("1234"));
        Assert.Equal("1/2/3/4/12345", service.GetKey("12345"));
        Assert.Equal("1/2/3/4/123456", service.GetKey("123456"));
    }
}