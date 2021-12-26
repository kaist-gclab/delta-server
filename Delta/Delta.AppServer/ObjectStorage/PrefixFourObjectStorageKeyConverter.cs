using System.Text;

namespace Delta.AppServer.ObjectStorage
{
    public class PrefixFourObjectStorageKeyConverter : IObjectStorageKeyConverter
    {
        private const int PrefixLength = 4;

        public string GetKey(string key)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < PrefixLength; i++)
            {
                builder.Append(i < key.Length ? key[i] : '$');
                builder.Append('/');
            }

            builder.Append(key);
            return builder.ToString();
        }
    }
}