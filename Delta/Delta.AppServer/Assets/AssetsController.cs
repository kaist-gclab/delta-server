using System;
using System.Threading.Tasks;
using Delta.AppServer.Encryption;
using Delta.AppServer.Stats;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace Delta.AppServer.Assets
{
    [ApiController]
    [Route(Delta.ApiRoot + "assets")]
    public class AssetsController : ControllerBase
    {
        private readonly AssetService _assetService;
        private readonly AssetMetadataService _assetMetadataService;
        private readonly EncryptionService _encryptionService;
        private readonly StatsService _statsService;
        private readonly IClock _clock;

        public AssetsController(AssetService assetService, AssetMetadataService assetMetadataService,
            EncryptionService encryptionService, StatsService statsService, IClock clock)
        {
            _assetService = assetService;
            _assetMetadataService = assetMetadataService;
            _encryptionService = encryptionService;
            _statsService = statsService;
            _clock = clock;
        }

        [HttpGet]
        public IActionResult GetAssets([FromQuery] string assetTagKey, [FromQuery] string assetTagValue)
        {
            _statsService.AddEvent(_clock.GetCurrentInstant(), "모델 목록 조회");

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

                var assetFormat = GetModelAssetFormat();
                var assetType = GetModelAssetType();
                var asset = await _assetService.AddAsset(assetFormat, assetType,
                    binary, null, null);
                AddImageUrl(asset);

                _assetMetadataService.UpdateAssetTag(asset, "Name", createAssetRequest.Name);
                if (createAssetRequest.Tag != null)
                {
                    _assetMetadataService.UpdateAssetTag(asset, "Tag", createAssetRequest.Tag);
                }


                _statsService.AddEvent(eventTimestamp, "모델 " + createAssetRequest.Name + " 추가");

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

        private AssetFormat GetModelAssetFormat()
        {
            var assetFormat = _assetMetadataService.GetAssetFormat("STL");
            if (assetFormat != null)
            {
                return assetFormat;
            }

            return _assetMetadataService.AddAssetFormat("STL", "STL", "STL");
        }

        private AssetFormat GetImageAssetFormat()
        {
            var assetFormat = _assetMetadataService.GetAssetFormat("BMP");
            if (assetFormat != null)
            {
                return assetFormat;
            }

            return _assetMetadataService.AddAssetFormat("BMP", "BMP", "BMP");
        }

        private AssetType GetModelAssetType()
        {
            var assetType = _assetMetadataService.GetAssetType("STL");
            if (assetType != null)
            {
                return assetType;
            }

            return _assetMetadataService.AddAssetType("STL", "STL");
        }

        private AssetType GetImageAssetType()
        {
            var assetType = _assetMetadataService.GetAssetType("BMP");
            if (assetType != null)
            {
                return assetType;
            }

            return _assetMetadataService.AddAssetType("BMP", "BMP");
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