using AssetHub.Samples;
using Xunit;

namespace AssetHub.EntityFrameworkCore.Domains;

[Collection(AssetHubTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AssetHubEntityFrameworkCoreTestModule>
{

}
