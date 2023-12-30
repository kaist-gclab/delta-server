using System.Collections.Generic;
using System.Threading.Tasks;
using CodeGen.Analysis;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Assets;

[ApiController]
[Route(Delta.ApiRoot + "asset-types")]
public class AssetTypesController : ControllerBase
{
    private readonly AssetMetadataService _assetMetadataService;

    public AssetTypesController(AssetMetadataService assetMetadataService)
    {
        _assetMetadataService = assetMetadataService;
    }

    [HttpGet]
    public IEnumerable<AssetType> GetAssetTypes()
    {
        return _assetMetadataService.GetAssetTypes();
    }

    [HttpPost]
    [Command]
    public async Task<IActionResult> Create([FromBody] CreateAssetTypeRequest createAssetTypeRequest)
    {
        await _assetMetadataService.AddAssetType(createAssetTypeRequest.Key, createAssetTypeRequest.Name);
        return Ok();
    }
}