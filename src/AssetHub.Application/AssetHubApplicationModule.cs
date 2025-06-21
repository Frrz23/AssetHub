using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using System.IO;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;

namespace AssetHub;

[DependsOn(
    typeof(AssetHubDomainModule),
    typeof(AssetHubApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
     typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpEmailingModule), 
    typeof(AbpBackgroundJobsModule)
    )]
public class AssetHubApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure(AssetManagementBlobContainers.AssetImportTemplates, container =>
            {
                container.UseFileSystem(files =>
                {
                    files.BasePath = Path.Combine(Directory.GetCurrentDirectory(), "blobs");
                });
            });
        });
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AssetHubApplicationModule>(); // ✅ important
        });

    }
}