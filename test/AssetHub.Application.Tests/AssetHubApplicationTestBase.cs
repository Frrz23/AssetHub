using Volo.Abp.Modularity;

namespace AssetHub;

public abstract class AssetHubApplicationTestBase<TStartupModule> : AssetHubTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
