using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace  AssetHub.Application.Asset

{
    public class AssetExcelTemplateSeeder : ITransientDependency
    {
        private readonly IBlobContainer _blobContainer;

        public AssetExcelTemplateSeeder(IBlobContainerFactory blobContainerFactory)
        {
            _blobContainer = blobContainerFactory.Create(AssetManagementBlobContainers.AssetImportTemplates);
        }

        public async Task SeedAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "AssetTemplate.xlsx");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Asset template not found", filePath);

            // ✅ Check if already uploaded
            if (await _blobContainer.ExistsAsync("AssetTemplate.xlsx"))
            {
                return; // Skip upload if already present
            }

            await _blobContainer.SaveAsync("AssetTemplate.xlsx", await File.ReadAllBytesAsync(filePath), overrideExisting: false);
        }
    }

    }
