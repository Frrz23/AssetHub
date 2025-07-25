using AssetHub.Asset;
using AssetHub.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AssetHub.Entities.Asset
{
    public interface IAssetAppService : IApplicationService
    {
        Task<AssetDto> CreateAsync(CreateAssetDto input);
        Task<AssetDto> GetAsync(Guid id);
        Task<List<AssetDto>> GetListAsync();
        Task<AssetDto> UpdateAsync(Guid id, CreateAssetDto input);
        Task DeleteAsync(Guid id);
        Task DeactivateAsync(Guid id);
        Task ReactivateAsync(Guid id);
        Task ApproveAsync(Guid id);
        Task<AssetDto> ApproveAsync(Guid id, ApproveAssetDto input);
        Task<FileDto> DownloadTemplateAsync();
        Task<FileDto> ExportToExcelAsync();
        Task ImportFromExcelAsync(byte[] fileBytes, string fileName);
        Task<AssetDashboardDto> GetDashboardStatsAsync();

    }
}
