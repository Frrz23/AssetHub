using AssetHub.Entities.AuditLog;
using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AssetHub.AuditLogService
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, Guid> _auditLogRepository;

        public AuditLogAppService(IRepository<AuditLog, Guid> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<List<AuditLogDto>> GetListAsync()
        {
            var logs = await _auditLogRepository.GetListAsync();
            return ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(logs);
        }

        public async Task<AuditLogDto> GetAsync(Guid id)
        {
            var log = await _auditLogRepository.GetAsync(id);
            return ObjectMapper.Map<AuditLog, AuditLogDto>(log);
        }
    }

}
