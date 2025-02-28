using Volo.Abp.Modularity;

namespace AssetHub;

/* Inherit from this class for your domain layer tests. */
public abstract class AssetHubDomainTestBase<TStartupModule> : AssetHubTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
