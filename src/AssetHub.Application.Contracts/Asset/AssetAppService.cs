﻿using AssetHub.Asset;
using AssetHub.AuditLogService;
using AssetHub.Common;
using AssetHub.Dashboard;
using AssetHub.Notifications;
using AssetHub.Services;
using AutoMapper.Internal.Mappers;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
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
using Volo.Abp.BackgroundJobs;
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
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly CustomEmailService _customEmailService;





        public AssetAppService(IRepository<Asset, Guid> assetRepository, IBlobContainerFactory blobContainerFactory, ITimeZoneConverter timeZoneConverter, IAuditLogService auditLogService, IBackgroundJobManager backgroundJobManager, CustomEmailService customEmailService)
        {
            _backgroundJobManager = backgroundJobManager;
            _assetRepository = assetRepository;
            _blobContainer = blobContainerFactory.Create(AssetManagementBlobContainers.AssetImportTemplates);
            _timeZoneConverter = timeZoneConverter;
            _auditLogService = auditLogService;
            _customEmailService = customEmailService;
        }
        public async Task NotifyAssetActionAsync(Asset asset, string action)
        {
            var subject = $"Asset {action}: {asset.AssetName}";
            var body = $@"
        <p>Asset <strong>{asset.AssetName}</strong> was <strong>{action.ToLower()}</strong> by {CurrentUser.UserName}.</p>
        <p><strong>Time:</strong> {Clock.Now}</p>
        <p><strong>Category:</strong> {asset.Category}</p>
        <p><strong>Department:</strong> {asset.Department}</p>
        <p><strong>Serial Number:</strong> {asset.SerialNumber}</p>";

            await _customEmailService.SendEmailAsync("aryankhatiwoda9@gmail.com", subject, body); // Replace with dynamic email if needed
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
            await NotifyAssetActionAsync(asset,"Created");
            return MapWithConvertedDates(asset); // 🔁 Use helper for timezone-adjusted DTO

        }


        public async Task<AssetDto> GetAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            return MapWithConvertedDates(asset);
        }


        public async Task<List<AssetDto>> GetListAsync()
        {
            // Return ALL assets, not just active ones
            var assets = await _assetRepository.GetListAsync();

            // Convert to DTOs with proper timezone conversion
            var dtos = new List<AssetDto>();
            foreach (var asset in assets)
            {
                dtos.Add(MapWithConvertedDates(asset));
            }

            return dtos;
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
            await NotifyAssetActionAsync(asset, "Updated");
            return MapWithConvertedDates(asset);
        }

        public async Task DeleteAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            await _auditLogService.LogAsync("Asset", "Delete", $"Asset '{asset.AssetName}' deleted.");
            await _assetRepository.DeleteAsync(id);
            await NotifyAssetActionAsync(asset, "Deleted");
        }
            


        
        public async Task DeactivateAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            asset.IsActive = false;
            await _assetRepository.UpdateAsync(asset);
            await _auditLogService.LogAsync("Asset", "Deactivate", $"Asset '{asset.AssetName}' deactivated.");

        }
        public async Task ReactivateAsync(Guid id)
        {
            var asset = await _assetRepository.GetAsync(id);
            asset.IsActive = true;
            await _assetRepository.UpdateAsync(asset);
            await _auditLogService.LogAsync("Asset", "Reactivate", $"Asset '{asset.AssetName}' reactivated.");
            await NotifyAssetActionAsync(asset, "Reactivated");
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
            await NotifyAssetActionAsync(asset, "Approved");


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
            dto.IsApproved = asset.IsApproved;

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

        public async Task ImportFromExcelAsync(byte[] fileBytes, string fileName)
        {
            using var stream = new MemoryStream(fileBytes);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1); // first sheet

            var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header

            foreach (var row in rows)
            {
                var assetName = row.Cell(1).GetString()?.Trim();
                var serialNumber = row.Cell(2).GetString()?.Trim();
                var category = row.Cell(3).GetString()?.Trim();
                var department = row.Cell(4).GetString()?.Trim();
                var receivedDateString = row.Cell(5).GetString()?.Trim();

                if (string.IsNullOrWhiteSpace(assetName) || string.IsNullOrWhiteSpace(serialNumber))
                    continue;

                if (await _assetRepository.AnyAsync(x => x.SerialNumber == serialNumber))
                    continue;

                DateTime.TryParse(receivedDateString, out var receivedDate);

                var asset = new Asset
                {
                    AssetName = assetName,
                    SerialNumber = serialNumber,
                    Category = category,
                    Department = department,
                    ReceivedDate = receivedDate.ToUniversalTime(),
                    IsApproved = false,
                    RequestedById = CurrentUser.Id,
                    IsActive = true
                };

                await _assetRepository.InsertAsync(asset, autoSave: true);
                await _auditLogService.LogAsync("Asset", "Import", $"Imported asset '{asset.AssetName}' from Excel.");
            }
        }
        public async Task<AssetDashboardDto> GetDashboardStatsAsync()
        {
            var allAssets = await _assetRepository.GetListAsync();

            var dto = new AssetDashboardDto
            {
                TotalAssets = allAssets.Count,
                ActiveAssets = allAssets.Count(x => x.IsActive),
                ApprovedAssets = allAssets.Count(x => x.IsApproved),
                UnapprovedAssets = allAssets.Count(x => !x.IsApproved),
                CategoryCounts = allAssets
                    .Where(x => !string.IsNullOrWhiteSpace(x.Category))
                    .GroupBy(x => x.Category)
                    .Select(g => new AssetCategoryCountDto { Category = g.Key, Count = g.Count() })
                    .ToList(),
                DepartmentCounts = allAssets
                    .Where(x => !string.IsNullOrWhiteSpace(x.Department))
                    .GroupBy(x => x.Department)
                    .Select(g => new AssetDepartmentCountDto { Department = g.Key, Count = g.Count() })
                    .ToList()
            };

            return dto;
        }



    }
}
