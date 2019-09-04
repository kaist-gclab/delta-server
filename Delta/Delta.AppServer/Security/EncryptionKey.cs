namespace Delta.AppServer.Security
{
    public class EncryptionKey
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }
    }
}