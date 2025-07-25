using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AssetHub.AssetAssignments
{
    public interface IAssetAssignmentService : IApplicationService
    {
        Task AssignAsync(CreateAssetAssignmentDto input);
        Task<List<AssetAssignmentDto>> GetHistoryByAssetIdAsync(Guid assetId);
    }

}
