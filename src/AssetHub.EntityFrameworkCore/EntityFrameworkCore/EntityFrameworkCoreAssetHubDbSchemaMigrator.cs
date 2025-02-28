using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AssetHub.Data;
using Volo.Abp.DependencyInjection;

namespace AssetHub.EntityFrameworkCore;

public class EntityFrameworkCoreAssetHubDbSchemaMigrator
    : IAssetHubDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAssetHubDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the AssetHubDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AssetHubDbContext>()
            .Database
            .MigrateAsync();
    }
}
