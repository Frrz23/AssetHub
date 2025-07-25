using AssetHub.AuditLogService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace AssetHub.Controllers
{
    [Route("api/app/audit-log")]
    public class AuditLogController : AbpController
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Get a list of all AuditLogs
        [HttpGet]
        public async Task<ActionResult> GetListAsync()
        {
            var result = await _auditLogAppService.GetListAsync();
            return Ok(result);
        }

        // Get a single AuditLog by ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(Guid id)
        {
            var result = await _auditLogAppService.GetAsync(id);
            return Ok(result);
        }
    }
}