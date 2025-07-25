using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp;
using AssetHub.Entities.Tag;
using AssetHub.Application.Contracts.Tags;

namespace AssetHub.Controllers.Tags
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Tag")]
    [Route("api/app/tags")]
    public class TagController : AbpController, ITagAppService
    {
        private readonly ITagAppService _tagAppService;

        public TagController(ITagAppService tagAppService)
        {
            _tagAppService = tagAppService;
        }

        [HttpPost]
        public Task<TagDto> CreateAsync(CreateTagDto input) => _tagAppService.CreateAsync(input);

        [HttpGet("{id}")]
        public Task<TagDto> GetAsync(Guid id) => _tagAppService.GetAsync(id);

        [HttpGet]
        public async Task<List<TagDto>> GetListAsync()
        {
            return await _tagAppService.GetListAsync();
        }

        [HttpPut("{id}")]
        public Task<TagDto> UpdateAsync(Guid id, CreateTagDto input) => _tagAppService.UpdateAsync(id, input);

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id) => _tagAppService.DeleteAsync(id);

    }
}
