using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Assets
{
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
    }
}