namespace Delta.AppServer.ObjectStorage
{
    public interface IObjectStorageKeyConverter
    {
        string GetKey(string key);
    }
}