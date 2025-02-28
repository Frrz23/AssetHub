using Volo.Abp.Modularity;

namespace AssetHub;

[DependsOn(
    typeof(AssetHubDomainModule),
    typeof(AssetHubTestBaseModule)
)]
public class AssetHubDomainTestModule : AbpModule
{

}
