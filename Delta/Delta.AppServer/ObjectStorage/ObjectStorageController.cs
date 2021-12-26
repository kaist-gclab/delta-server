using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delta.AppServer.ObjectStorage
{
    [ApiController]
    [Route(Delta.ApiRoot + "object-storage")]
    public class ObjectStorageController : ControllerBase
    {
        private readonly IObjectStorageService _objectStorageService;

        public ObjectStorageController(IObjectStorageService objectStorageService)
        {
            _objectStorageService = objectStorageService;
        }

        [HttpGet("upload-url")]
        public async Task<UploadTicket> GetUploadUrl()
        {
            var storeKey = Guid.NewGuid().ToString();
            var url = await _objectStorageService.GetPresignedUploadUrl(storeKey);
            return new UploadTicket(url, storeKey);
        }
    }
}