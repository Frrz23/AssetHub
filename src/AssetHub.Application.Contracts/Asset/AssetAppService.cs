using AssetHub.Asset;
using AssetHub.Common;
using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace AssetHub.Entities.Asset
{
    public class AssetAppService : ApplicationService, IAssetAppService
    {
        private readonly IRepository<Asset, Guid> _assetRepository;
        private readonly IBlobContainer _blobContainer;
        private readonly ITimeZoneConverter _timeZoneConverter;



        public AssetAppService(IRepository<Asset, Guid> assetRepository, IBlobContainerFactory blobContainerFactory, ITimeZoneConverter timeZoneConverter)
        {
            _assetRepository = assetRepository;
            _blobContainer = blobContainerFactory.Create(AssetManagementBlobContainers.AssetImportTemplates);
            _timeZoneConverter = timeZoneConverter;
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
            input.ReceivedDate = input.ReceivedDate.ToUniversalTime();

            var asset = ObjectMapper.Map<CreateAssetDto, Asset>(input);
            var currentUserId = CurrentUser.Id;

            asset.IsApproved = false;
            asset.RequestedById = currentUserId;

            asset = await _assetRepository.InsertAsync(asset, autoSave: true);
            return MapWithConvertedDates(asset); // 🔁 Use helper for timezone-adjusted DTO

        }


        public async Task<AssetDto> GetAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            return MapWithConvertedDates(asset);
        }


        public async Task<List<AssetDto>> GetListAsync()
        {
            var assets = await _assetRepository.GetListAsync(x => x.IsActive);
            return ObjectMapper.Map<List<Asset>, List<AssetDto>>(assets);
        }

        public async Task<AssetDto> UpdateAsync(Guid id, CreateAssetDto input)
        {
            var asset = await _assetRepository.GetAsync(id);
            var currentUserId = CurrentUser.Id;

            asset.IsApproved = false;
            asset.RequestedById = currentUserId;

            ObjectMapper.Map(input, asset); // updates the entity
            asset.ReceivedDate = input.ReceivedDate.ToUniversalTime();
            asset = await _assetRepository.UpdateAsync(asset);
            return MapWithConvertedDates(asset);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _assetRepository.DeleteAsync(id);
        }
        public async Task DeactivateAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            asset.IsActive = false;
            await _assetRepository.UpdateAsync(asset);
        }
        public async Task ApproveAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            asset.IsApproved = true;
            asset.ApprovedById = CurrentUser.Id;
            await _assetRepository.UpdateAsync(asset);
        }
        public async Task<AssetDto> ApproveAsync(Guid id, ApproveAssetDto input)
        {
            var asset = await _assetRepository.GetAsync(id);

            if (input.Approve)
            {
                asset.IsApproved = true;
                asset.ApprovedById = CurrentUser.Id;
                asset.ApprovedTime = Clock.Normalize(DateTime.UtcNow);
            }
            else
            {
                asset.IsApproved = false;
                asset.ApprovedById = null;
                asset.ApprovedTime = null;
                // Optionally log or store the rejection comment somewhere
            }

            asset = await _assetRepository.UpdateAsync(asset);
            return MapWithConvertedDates(asset);
        }
        public async Task<FileDto> DownloadTemplateAsync()
        {
            var fileName = "AssetTemplate.xlsx";

            if (!await _blobContainer.ExistsAsync(fileName))
            {
                throw new UserFriendlyException("Template not found in storage.");
            }

            var fileBytes = await _blobContainer.GetAllBytesAsync(fileName);
            return new FileDto
            {
                FileName = fileName,
                Content = fileBytes,
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
    

    private AssetDto MapWithConvertedDates(Asset asset)
        {
            var dto = ObjectMapper.Map<Asset, AssetDto>(asset);
            dto.ReceivedDate = _timeZoneConverter.ConvertToUserTime(asset.ReceivedDate);

            if (asset.ApprovedTime.HasValue)
            {
                dto.ApprovedTime = _timeZoneConverter.ConvertToUserTime(asset.ApprovedTime.Value);
            }

            return dto;
        }

    }
    }
