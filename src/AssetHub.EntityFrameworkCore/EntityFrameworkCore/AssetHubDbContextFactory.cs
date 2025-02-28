using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AssetHub.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AssetHubDbContextFactory : IDesignTimeDbContextFactory<AssetHubDbContext>
{
    public AssetHubDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        AssetHubEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<AssetHubDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new AssetHubDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AssetHub.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
