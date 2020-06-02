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
        private readonly EncryptionService _encryptionService;

        public AssetsController(AssetService assetService, AssetMetadataService assetMetadataService,
            EncryptionService encryptionService)
        {
            _assetService = assetService;
            _assetMetadataService = assetMetadataService;
            _encryptionService = encryptionService;
        }

        [HttpGet]
        public IActionResult GetAssets([FromQuery] string assetTagKey, [FromQuery] string assetTagValue)
        {
            if (assetTagKey == null && assetTagValue == null)
            {
                return Ok(_assetMetadataService.GetAssets());
            }

            return Ok(_assetMetadataService.FindByAssetTag(assetTagKey, assetTagValue));
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
                var assetFormat = GetAssetFormat(createAssetRequest.AssetFormatKey);
                var assetType = GetAssetType(createAssetRequest.AssetTypeKey);
                var encryptionKey = GetEncryptionKey(createAssetRequest.EncryptionKeyName);
                var asset = await _assetService.AddAsset(assetFormat, assetType, createAssetRequest.Content,
                    encryptionKey, null);
                return Ok(asset);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        [HttpPost("formats")]
        public IActionResult CreateAssetFormat(CreateAssetFormatRequest createAssetFormatRequest)
        {
            return Ok(_assetMetadataService.AddAssetFormat(
                createAssetFormatRequest.Key,
                createAssetFormatRequest.Name,
                createAssetFormatRequest.Description));
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

        private AssetFormat GetAssetFormat(string assetFormatKey)
        {
            if (assetFormatKey == null)
            {
                return null;
            }

            var assetFormat = _assetMetadataService.GetAssetFormat(assetFormatKey);
            if (assetFormat == null)
            {
                throw new ArgumentException();
            }

            return assetFormat;
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

        private EncryptionKey GetEncryptionKey(string encryptionKeyName)
        {
            if (encryptionKeyName == null)
            {
                return null;
            }

            var encryptionKey = _encryptionService.GetEncryptionKey(encryptionKeyName);
            if (encryptionKey == null)
            {
                throw new ArgumentException();
            }

            return encryptionKey;
        }
    }
}