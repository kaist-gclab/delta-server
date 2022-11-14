using System.IO;
using System.IO.Compression;

namespace Delta.AppServer.Assets;

public class CompressionService
{
    public byte[] Compress(byte[] data)
    {
        using var outputStream = new MemoryStream();
        using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
        {
            using var inputStream = new MemoryStream(data);
            inputStream.CopyTo(compressionStream);
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