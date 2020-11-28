using System;
using System.Threading.Tasks;
using Delta.AppServer.Encryption;
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
        [RequestSizeLimit(200 * 1024 * 1024)]
        public async Task<IActionResult> CreateAsset(CreateAssetRequest createAssetRequest)
        {
            if (createAssetRequest.Content == null)
            {
                return BadRequest();
            }

            try
            {
                var assetType = GetAssetType(createAssetRequest.AssetTypeKey);
                var encryptionKey = GetEncryptionKey(createAssetRequest.EncryptionKeyName);
                var asset = await _assetService.AddAsset(assetType, createAssetRequest.Content,
                    encryptionKey, null);
                return Ok(asset);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }


        [HttpPost("model")]
        [RequestSizeLimit(200 * 1024 * 1024)]
        public async Task<IActionResult> CreateModelAsset([FromBody] CreateModelAssetRequest createAssetRequest)
        {
            if (createAssetRequest.Content == null)
            {
                return BadRequest();
            }

            try
            {
                var eventTimestamp = createAssetRequest.EventTimestamp;

                var b = createAssetRequest.Content.IndexOf("base64,", StringComparison.Ordinal);
                createAssetRequest.Content = createAssetRequest.Content.Substring(b + 7);
                var binary = Convert.FromBase64String(createAssetRequest.Content);

                void AddImageUrl(Asset asset)
                {
                    _assetMetadataService.UpdateAssetTag(asset, "Image",
                        "https://image.delta-test.cqcqcqde.com/" + asset.Id + ".bmp");
                }

                var assetType = GetModelAssetType();
                var asset = await _assetService.AddAsset(assetType,
                    binary, null, null);
                AddImageUrl(asset);

                _assetMetadataService.UpdateAssetTag(asset, "Name", createAssetRequest.Name);
                if (createAssetRequest.Tag != null)
                {
                    _assetMetadataService.UpdateAssetTag(asset, "Tag", createAssetRequest.Tag);
                }

                return Ok(asset);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost("{assetId:long}/tags")]
        public IActionResult UpdateAssetTag(long assetId, [FromBody] UpdateAssetTagRequest updateAssetTagRequest)
        {
            var asset = _assetMetadataService.GetAsset(assetId);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(_assetMetadataService.UpdateAssetTag(asset,
                updateAssetTagRequest.Key, updateAssetTagRequest.Value));
        }

        [HttpGet("{assetId:long}/download")]
        public async Task<IActionResult> DownloadAsset(long assetId)
        {
            var asset = _assetMetadataService.GetAsset(assetId);
            if (asset == null)
            {
                return NotFound();
            }

            var bytes = await _assetService.ReadAssetContent(asset);
            return Ok(bytes);
        }

        private AssetType GetAssetType(string assetTypeKey)
        {
            if (assetTypeKey == null)
            {
                return null;
            }

            var assetType = _assetMetadataService.GetAssetType(assetTypeKey);
            if (assetType == null)
            {
                throw new ArgumentException();
            }

            return assetType;
        }
    }
}