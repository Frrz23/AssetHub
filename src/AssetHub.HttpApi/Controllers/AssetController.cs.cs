using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace AssetHub.Controllers
{
    [Route("api/assets")]
    [ApiController]
    public class AssetController : AbpController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = new List<string> { "Laptop", "Phone", "Printer" };
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            return Ok($"Asset with ID: {id}");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] string assetName)
        {
            return Created("", $"Asset {assetName} created.");
        }
    }
}
