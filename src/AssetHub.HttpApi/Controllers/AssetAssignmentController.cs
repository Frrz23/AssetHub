using AssetHub.AssetAssignments;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHub.Controllers
{
    [Route("api/assignments")]
    public class AssetAssignmentController : ControllerBase
    {
        private readonly IAssetAssignmentService _service;

        public AssetAssignmentController(IAssetAssignmentService service)
        {
            _service = service;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] CreateAssetAssignmentDto input)
        {
            await _service.AssignAsync(input);
            return NoContent();
        }

        [HttpGet("history/{assetId}")]
        public async Task<List<AssetAssignmentDto>> GetHistory(Guid assetId)
        {
            return await _service.GetHistoryByAssetIdAsync(assetId);
        }
    }

}
