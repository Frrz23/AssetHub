using Volo.Abp.Settings;

namespace AssetHub.Settings;

public class AssetHubSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AssetHubSettings.MySetting1));
    }
}
