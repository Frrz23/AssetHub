using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace AssetHub.Entities.Tag
{
    public class TagAppService : ApplicationService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
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