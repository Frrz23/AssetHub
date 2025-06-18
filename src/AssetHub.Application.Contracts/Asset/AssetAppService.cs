using AssetHub.Asset;
using AssetHub.AuditLogService;
using AssetHub.Common;
using AutoMapper.Internal.Mappers;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IAuditLogService _auditLogService;




        public AssetAppService(IRepository<Asset, Guid> assetRepository, IBlobContainerFactory blobContainerFactory, ITimeZoneConverter timeZoneConverter, IAuditLogService auditLogService)
        {
            _assetRepository = assetRepository;
            _blobContainer = blobContainerFactory.Create(AssetManagementBlobContainers.AssetImportTemplates);
            _timeZoneConverter = timeZoneConverter;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync("Asset", "Create", $"Asset '{input.AssetName}' created.");
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
            await _auditLogService.LogAsync("Asset", "Update", $"Asset '{input.AssetName}' updated.");
            return MapWithConvertedDates(asset);
        }

        public async Task DeleteAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            await _auditLogService.LogAsync("Asset", "Delete", $"Asset '{asset.AssetName}' deleted.");
            await _assetRepository.DeleteAsync(id);

        }
        public async Task DeactivateAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            asset.IsActive = false;
            await _assetRepository.UpdateAsync(asset);
            await _auditLogService.LogAsync("Asset", "Deactivate", $"Asset '{asset.AssetName}' deactivated.");

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
            await _auditLogService.LogAsync("Asset", "Approve", $"Asset '{asset.AssetName}' was {(input.Approve ? "approved" : "rejected")}.");

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


        public async Task<FileDto> ExportToExcelAsync()
        {
            var assets = await _assetRepository.GetListAsync(x => x.IsActive);

            // 1) Create a new workbook and sheet
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Assets");

            // 2) Create header row
            var header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("Asset Name");
            header.CreateCell(1).SetCellValue("Serial Number");
            header.CreateCell(2).SetCellValue("Category");
            header.CreateCell(3).SetCellValue("Department");
            header.CreateCell(4).SetCellValue("Received Date");
            header.CreateCell(5).SetCellValue("Is Approved");

            // 3) Fill in data rows
            for (int i = 0; i < assets.Count; i++)
            {
                var row = sheet.CreateRow(i + 1);
                var asset = assets[i];
                row.CreateCell(0).SetCellValue(asset.AssetName);
                row.CreateCell(1).SetCellValue(asset.SerialNumber);
                row.CreateCell(2).SetCellValue(asset.Category);
                row.CreateCell(3).SetCellValue(asset.Department);
                row.CreateCell(4).SetCellValue(
                    _timeZoneConverter
                        .ConvertToUserTime(asset.ReceivedDate)
                        .ToString("yyyy-MM-dd")
                );
                row.CreateCell(5).SetCellValue(asset.IsApproved ? "Yes" : "No");
            }

            // 4) Auto-size columns (optional)
            for (int col = 0; col < 6; col++)
            {
                sheet.AutoSizeColumn(col);
            }

            // 5) Write to memory stream
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                bytes = ms.ToArray();
            }

            return new FileDto
            {
                FileName = $"AssetExport_{Clock.Now:yyyyMMdd_HHmmss}.xlsx",
                Content = bytes,
                ContentType =
                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }




    }
}
