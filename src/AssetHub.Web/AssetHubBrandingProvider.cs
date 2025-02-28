using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using AssetHub.Localization;

namespace AssetHub.Web;

[Dependency(ReplaceServices = true)]
public class AssetHubBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AssetHubResource> _localizer;

    public AssetHubBrandingProvider(IStringLocalizer<AssetHubResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
