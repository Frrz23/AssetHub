using AssetHub.Asset;
using AssetHub.Entities.Asset;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace AssetHub.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetController : AbpController
    {
        private readonly IAssetAppService _assetAppService;

        public AssetController(IAssetAppService assetAppService)
        {
            _assetAppService = assetAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _assetAppService.GetListAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            var asset = await _assetAppService.GetAsync(id);
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetDto input)
        {
            var asset = await _assetAppService.CreateAsync(input);
            return CreatedAtAction(nameof(GetAssetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(Guid id, [FromBody] CreateAssetDto input)
        {
            var asset = await _assetAppService.UpdateAsync(id, input);
            return Ok(asset);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            await _assetAppService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateAsset(Guid id)
        {
            await _assetAppService.DeactivateAsync(id);
            return NoContent();
        }
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveAsset(Guid id)
        {
            await _assetAppService.ApproveAsync(id);
            return NoContent();
        }

        [HttpGet("download-template")]
        public async Task<IActionResult> DownloadTemplate()
        {
            var file = await _assetAppService.DownloadTemplateAsync();
            return File(file.Content, file.ContentType, file.FileName);
        }
        [HttpGet("export")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public async Task<IActionResult> ExportToExcel()
        {
            var fileDto = await _assetAppService.ExportToExcelAsync();

            // This returns a FileContentResult so Swagger/browser will download it
            return File(
                fileDto.Content,
                fileDto.ContentType,
                fileDto.FileName
            );
        }
        [HttpPost("import-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportFromExcelAsync([FromForm] ImportExcelInputDto input)
        {
            var file = input.File;

            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("Invalid file.");
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            await _assetAppService.ImportFromExcelAsync(stream.ToArray(), file.FileName);
            return NoContent();
        }

    }
}