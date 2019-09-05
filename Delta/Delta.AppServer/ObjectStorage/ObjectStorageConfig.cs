using Microsoft.Extensions.Configuration;

namespace Delta.AppServer.ObjectStorage
{
    public class ObjectStorageConfig
    {
        private readonly IConfiguration _configuration;

        public ObjectStorageConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        
        public string Endpoint => _configuration["ObjectStorage:Endpoint"];
        public string AccessKey => _configuration["ObjectStorage:AccessKey"];
        public string SecretKey => _configuration["ObjectStorage:SecretKey"];
        public string Bucket => _configuration["ObjectStorage:Bucket"];

    }
}