using AssetHub.Entities.AuditLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace AssetHub.AuditLogService
{
    public class AuditLogService : ApplicationService, IAuditLogService
    {
        private readonly IRepository<AuditLog, Guid> _logRepo;

        public AuditLogService(IRepository<AuditLog, Guid> logRepo)
        {
            _logRepo = logRepo;
        }

        public async Task LogAsync(string entityName, string action, string description = null)
        {
            var log = new AuditLog(
                entityName,
                action,
                CurrentUser?.Email ?? "System",
                Clock.Now, // UTC
                description
            );
            await _logRepo.InsertAsync(log, autoSave: true);
        }
        public async Task<List<AuditLogDto>> GetListAsync()
        {
            var logs = await _logRepo.GetListAsync();
            return logs.OrderByDescending(x => x.PerformedAt)
                       .Select(x => new AuditLogDto
                       {
                           EntityName = x.EntityName,
                           Action = x.Action,
                           PerformedBy = x.PerformedBy,
                           PerformedAt = x.PerformedAt,
                           Description = x.Description
                       }).ToList();
        }
    }

}
