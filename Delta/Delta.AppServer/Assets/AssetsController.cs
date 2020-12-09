using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.Assets
{
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
        public IActionResult GetAssets()
        {
            return Ok(_assetMetadataService.GetAssets());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset(CreateAssetRequest createAssetRequest)
        {
            if (createAssetRequest.CreateAssetTagRequest == null)
            {
                return BadRequest();
            }

            try
            {
                var asset = await _assetService.AddAsset(createAssetRequest);
                return Ok(asset);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAsset(long id)
        {
            var response = await _assetService.GetAsset(id);
            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}