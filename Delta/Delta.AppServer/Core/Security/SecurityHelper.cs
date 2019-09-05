using System;

namespace Delta.AppServer.Core.Security
{
    public static class SecurityHelper
    {
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}