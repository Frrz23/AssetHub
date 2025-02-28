using AssetHub.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AssetHub.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AssetHubController : AbpControllerBase
{
    protected AssetHubController()
    {
        LocalizationResource = typeof(AssetHubResource);
    }
}
