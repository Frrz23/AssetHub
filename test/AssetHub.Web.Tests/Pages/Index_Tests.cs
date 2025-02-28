using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AssetHub.Pages;

[Collection(AssetHubTestConsts.CollectionDefinitionName)]
public class Index_Tests : AssetHubWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
