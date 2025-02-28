using AssetHub.Localization;
using Volo.Abp.Application.Services;

namespace AssetHub;

/* Inherit your application services from this class.
 */
public abstract class AssetHubAppService : ApplicationService
{
    protected AssetHubAppService()
    {
        LocalizationResource = typeof(AssetHubResource);
    }
}
