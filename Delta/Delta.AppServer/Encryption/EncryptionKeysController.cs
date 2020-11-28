using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Encryption
{
    [ApiController]
    [Route(Delta.ApiRoot + "encryption-keys")]
    public class EncryptionKeysController : ControllerBase
    {
        private readonly EncryptionService _encryptionService;

        public EncryptionKeysController(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpGet]
        public IEnumerable<EncryptionKey> GetEncryptionKeys()
        {
            return _encryptionService.GetEncryptionKeys();
        }

        [HttpPost]
        public CreateEncryptionKeyResponse Create([FromBody] CreateEncryptionKeyRequest createEncryptionKeyRequest)
        {
            var key = _encryptionService.AddEncryptionKey(createEncryptionKeyRequest);
            return new CreateEncryptionKeyResponse
            {
                EncryptionKey = key,
                Value = key.Value
            };
        }
    }
}