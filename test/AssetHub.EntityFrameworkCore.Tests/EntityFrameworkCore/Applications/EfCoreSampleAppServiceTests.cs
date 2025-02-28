using AssetHub.Samples;
using Xunit;

namespace AssetHub.EntityFrameworkCore.Applications;

[Collection(AssetHubTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AssetHubEntityFrameworkCoreTestModule>
{

}
