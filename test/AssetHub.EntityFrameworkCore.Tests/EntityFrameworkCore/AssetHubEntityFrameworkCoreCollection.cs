using Xunit;

namespace AssetHub.EntityFrameworkCore;

[CollectionDefinition(AssetHubTestConsts.CollectionDefinitionName)]
public class AssetHubEntityFrameworkCoreCollection : ICollectionFixture<AssetHubEntityFrameworkCoreFixture>
{

}
