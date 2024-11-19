using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Assets;

[ApiController]
[Route(Delta.ApiRoot + "assets")]
public class AssetsController(
    AssetService assetService,
    AssetMetadataService assetMetadataService)
    : ControllerBase
{
    [HttpGet]
    public IEnumerable<Asset> GetAssets()
    {
        return assetMetadataService.GetAssets();
    }

    [HttpPost]
    [Command]
    public async Task<IActionResult> CreateAsset(CreateAssetRequest createAssetRequest)
    {
        await assetService.AddAsset(createAssetRequest);
        return Ok();
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<GetAssetResponse?>> GetAsset(long id)
    {
        var response = await assetService.GetAsset(id);
        if (response == null)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}