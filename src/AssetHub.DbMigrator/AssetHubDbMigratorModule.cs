using AssetHub.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AssetHub.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AssetHubEntityFrameworkCoreModule),
    typeof(AssetHubApplicationContractsModule)
)]
public class AssetHubDbMigratorModule : AbpModule
{
}
