using AssetHub.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AssetHub.Web.Pages;

public abstract class AssetHubPageModel : AbpPageModel
{
    protected AssetHubPageModel()
    {
        LocalizationResourceType = typeof(AssetHubResource);
    }
}
