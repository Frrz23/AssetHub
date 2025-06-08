using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AssetHub.Entities.Tag
{
    public interface ITagAppService : IApplicationService
    {
        Task<TagDto> CreateAsync(CreateTagDto input);
        Task<TagDto> GetAsync(Guid id);
        Task<List<TagDto>> GetListAsync();
        Task<TagDto> UpdateAsync(Guid id, CreateTagDto input);
        Task DeleteAsync(Guid id);
    }
}
