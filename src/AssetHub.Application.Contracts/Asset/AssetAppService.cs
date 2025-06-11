using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AssetHub.Entities.Asset
{
    public class AssetAppService : ApplicationService, IAssetAppService
    {
        private readonly IRepository<Asset, Guid> _assetRepository;

        public AssetAppService(IRepository<Asset, Guid> assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<AssetDto> CreateAsync(CreateAssetDto input)
        {
            input.AssetName = input.AssetName?.Trim();
            input.SerialNumber = input.SerialNumber?.Trim();
            input.Category = input.Category?.Trim();
            input.Department = input.Department?.Trim();

            // Check for unique SerialNumber
            var existing = await _assetRepository.FirstOrDefaultAsync(x => x.SerialNumber == input.SerialNumber);
            if (existing != null)
            {
                throw new UserFriendlyException("An asset with this Serial Number already exists.");
            }

            var asset = ObjectMapper.Map<CreateAssetDto, Asset>(input);
            asset = await _assetRepository.InsertAsync(asset, autoSave: true);
            return ObjectMapper.Map<Asset, AssetDto>(asset);
        }


        public async Task<AssetDto> GetAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            return ObjectMapper.Map<Asset, AssetDto>(asset);
        }

        public async Task<List<AssetDto>> GetListAsync()
        {
            var assets = await _assetRepository.GetListAsync();
            return ObjectMapper.Map<List<Asset>, List<AssetDto>>(assets);
        }

        public async Task<AssetDto> UpdateAsync(Guid id, CreateAssetDto input)
        {
            var asset = await _assetRepository.GetAsync(id);
            ObjectMapper.Map(input, asset); // updates the entity
            asset = await _assetRepository.UpdateAsync(asset);
            return ObjectMapper.Map<Asset, AssetDto>(asset);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _assetRepository.DeleteAsync(id);
        }
    }
}