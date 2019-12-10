using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Encryption
{
    [ApiController]
    [Route(Delta.ApiRoot + "encryptionKeys")]
    public class EncryptionKeysController : ControllerBase
    {
        private readonly EncryptionService _encryptionService;

        public EncryptionKeysController(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpPost]
        public EncryptionKey Create([FromBody] EncryptionKeyCreateRequest encryptionKeyCreateRequest)
        {
            var key = _encryptionService.AddEncryptionKey(encryptionKeyCreateRequest.Name);
            if (encryptionKeyCreateRequest.Enabled)
            {
                _encryptionService.EnableKey(key);
            }

            return key;
        }
    }

    public class EncryptionKeyCreateRequest
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}