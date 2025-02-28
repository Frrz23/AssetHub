using Volo.Abp.Modularity;

namespace AssetHub;

[DependsOn(
    typeof(AssetHubApplicationModule),
    typeof(AssetHubDomainTestModule)
)]
public class AssetHubApplicationTestModule : AbpModule
{

}
