using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Encryption;

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
    public async Task<ActionResult<CreateEncryptionKeyResponse>> Create(
        [FromBody] CreateEncryptionKeyRequest createEncryptionKeyRequest)
    {
        var response = await _encryptionService.AddEncryptionKey(createEncryptionKeyRequest);
        if (response == null)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}