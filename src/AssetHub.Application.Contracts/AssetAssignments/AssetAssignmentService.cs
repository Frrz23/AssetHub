using AssetHub.AuditLogService;
using AssetHub.Entities.Asset;
using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace AssetHub.AssetAssignments
{
    public class AssetAssignmentService : ApplicationService, IAssetAssignmentService
    {
        private readonly IRepository<AssetAssignmentHistory, Guid> _assignmentRepository;
        private readonly IAuditLogService _auditLogService;

        public AssetAssignmentService(
            IRepository<AssetAssignmentHistory, Guid> assignmentRepository,
            IAuditLogService auditLogService)
        {
            _assignmentRepository = assignmentRepository;
            _auditLogService = auditLogService;
        }

        public async Task AssignAsync(CreateAssetAssignmentDto input)
        {
            // Close any existing active assignment for this asset
            var active = await _assignmentRepository.FirstOrDefaultAsync(x => x.AssetId == input.AssetId && x.IsActive);
            if (active != null)
            {
                active.IsActive = false;
                active.ReturnedDate = Clock.Now;
                await _assignmentRepository.UpdateAsync(active);
            }

            var assignment = new AssetAssignmentHistory
            {
                CreatorId = GuidGenerator.Create(),
                AssetId = input.AssetId,
                AssignedTo = input.AssignedTo.Trim(),
                AssignedDate = input.AssignedDate.ToUniversalTime(),
                IsActive = true
            };

            await _assignmentRepository.InsertAsync(assignment, autoSave: true);
            await _auditLogService.LogAsync("AssetAssignment", "Assign", $"Asset {input.AssetId} assigned to {input.AssignedTo}.");
        }

        public async Task<List<AssetAssignmentDto>> GetHistoryByAssetIdAsync(Guid assetId)
        {
            var history = await _assignmentRepository.GetListAsync(x => x.AssetId == assetId);
            return ObjectMapper.Map<List<AssetAssignmentHistory>, List<AssetAssignmentDto>>(history);
        }
    }

}
