using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetHub.Application.Contracts.Tags;
using AssetHub.Entities.Tag;             // Your Tag entity
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;     // <-- Add this

namespace AssetHub.Application.Tags
{
    public class TagAppService : ApplicationService, ITagAppService
    {
        // Change this…
        // private readonly ITagRepository _tagRepository;
        // public TagAppService(ITagRepository tagRepository)

        // …to this:
        private readonly IRepository<Tag, Guid> _tagRepository;
        public TagAppService(IRepository<Tag, Guid> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<TagDto> CreateAsync(CreateTagDto input)
        {
            var tag = ObjectMapper.Map<CreateTagDto, Tag>(input);
            tag = await _tagRepository.InsertAsync(tag, autoSave: true);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task<TagDto> GetAsync(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task<List<TagDto>> GetListAsync()
        {
            var tags = await _tagRepository.GetListAsync();
            return ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
        }

        public async Task<TagDto> UpdateAsync(Guid id, CreateTagDto input)
        {
            var tag = await _tagRepository.GetAsync(id);
            ObjectMapper.Map(input, tag);
            tag = await _tagRepository.UpdateAsync(tag);
            return ObjectMapper.Map<Tag, TagDto>(tag);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _tagRepository.DeleteAsync(id);
        }
    }
}
