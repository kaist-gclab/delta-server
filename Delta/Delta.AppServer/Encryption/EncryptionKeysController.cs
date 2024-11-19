using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Encryption;

[ApiController]
[Route(Delta.ApiRoot + "encryption-keys")]
public class EncryptionKeysController(EncryptionService encryptionService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<EncryptionKeyView>> GetEncryptionKeys()
    {
        return await encryptionService.GetEncryptionKeys();
    }

    [HttpPost]
    public async Task<ActionResult<CreateEncryptionKeyResponse>> Create(
        [FromBody] CreateEncryptionKeyRequest createEncryptionKeyRequest)
    {
        var response = await encryptionService.AddEncryptionKey(createEncryptionKeyRequest);
        if (response == null)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}