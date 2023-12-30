using System;
using System.Security.Cryptography;
using System.Text;

namespace Delta.AppServer.Core.Security;

public static class SecurityHelper
{
    public static string ComputeSHA256(this string text)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return hash.ToHexString();
    }

    public static string ToHexString(this byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
}