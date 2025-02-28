using Microsoft.AspNetCore.Builder;
using AssetHub;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("AssetHub.Web.csproj"); 
await builder.RunAbpModuleAsync<AssetHubWebTestModule>(applicationName: "AssetHub.Web");

public partial class Program
{
}
