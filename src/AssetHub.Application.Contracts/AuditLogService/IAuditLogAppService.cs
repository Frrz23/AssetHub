using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AssetHub.AuditLogService
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<List<AuditLogDto>> GetListAsync();
        Task<AuditLogDto> GetAsync(Guid id);
        // Add more methods if needed
    }
}
