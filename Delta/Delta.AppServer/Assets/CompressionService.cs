using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Delta.AppServer.Assets;

public class CompressionService
{
    public async Task<byte[]> Compress(byte[] data)
    {
        await using var outputStream = new MemoryStream();
        await using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
        {
            using var inputStream = new MemoryStream(data);
            await inputStream.CopyToAsync(compressionStream);
        }

        return outputStream.ToArray();
    }

    public byte[] Decompress(byte[] data)
    {
        using var inputStream = new MemoryStream(data);
        using var outputStream = new MemoryStream();
        using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
        {
            compressionStream.CopyTo(outputStream);
        }

        return outputStream.ToArray();
    }
}