using System.Threading.Tasks;

namespace AssetHub.Data;

public interface IAssetHubDbSchemaMigrator
{
    Task MigrateAsync();
}
