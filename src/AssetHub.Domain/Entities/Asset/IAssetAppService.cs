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
    }
}
