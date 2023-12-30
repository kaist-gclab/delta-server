using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Assets;

[ApiController]
[Route(Delta.ApiRoot + "assets")]
public class AssetsController : ControllerBase
{
    private readonly AssetService _assetService;
    private readonly AssetMetadataService _assetMetadataService;

    public AssetsController(AssetService assetService,
        AssetMetadataService assetMetadataService)
    {
        _assetService = assetService;
        _assetMetadataService = assetMetadataService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Asset>> GetAssets()
    {
        return Ok(_assetMetadataService.GetAssets());
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsset(CreateAssetRequest createAssetRequest)
    {
        await _assetService.AddAsset(createAssetRequest);
        return Ok();
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<GetAssetResponse?>> GetAsset(long id)
    {
        var response = await _assetService.GetAsset(id);
        if (response == null)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}