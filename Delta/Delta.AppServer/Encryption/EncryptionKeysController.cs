using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
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

    [HttpGet("{id:long}")]
    public async Task<ActionResult<EncryptionKeyView>> GetEncryptionKey(long id)
    {
        var encryptionKey = await encryptionService.GetEncryptionKey(id);
        if (encryptionKey == null)
        {
            return NotFound();
        }

        return Ok(encryptionKey);
    }

    [HttpPost]
    [Command]
    public async Task<ActionResult> Create(
        [FromBody] CreateEncryptionKeyRequest createEncryptionKeyRequest)
    {
        await encryptionService.AddEncryptionKey(createEncryptionKeyRequest);
        return Ok();
    }
    
    [HttpDelete("{id:long}")]
    [Command]
    public async Task<ActionResult> Delete(long id)
    {
        await encryptionService.DeleteEncryptionKey(id);
        return Ok();
    }
}