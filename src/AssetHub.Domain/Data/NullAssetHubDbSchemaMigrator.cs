using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AssetHub.Data;

/* This is used if database provider does't define
 * IAssetHubDbSchemaMigrator implementation.
 */
public class NullAssetHubDbSchemaMigrator : IAssetHubDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
