using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Data;

public class OnlineCoursesDbMigrationService : ITransientDependency
{
    public ILogger<OnlineCoursesDbMigrationService> Logger { get; set; }

    private readonly IDataSeeder _dataSeeder;
    private readonly OnlineCoursesEFCoreDbSchemaMigrator _dbSchemaMigrator;

    public OnlineCoursesDbMigrationService(
        IDataSeeder dataSeeder,
        OnlineCoursesEFCoreDbSchemaMigrator dbSchemaMigrator)
    {
        _dataSeeder = dataSeeder;
        _dbSchemaMigrator = dbSchemaMigrator;

        Logger = NullLogger<OnlineCoursesDbMigrationService>.Instance;
    }

    public async Task MigrateAsync()
    {
        var initialMigrationAdded = AddInitialMigrationIfNotExist();

        if (initialMigrationAdded)
        {
            return;
        }

        Logger.LogInformation("Started database migrations...");

        await MigrateDatabaseSchemaAsync();
        await SeedDataAsync();

        Logger.LogInformation("Successfully completed database migrations.");
        Logger.LogInformation("You can safely end this process...");
    }

    private async Task MigrateDatabaseSchemaAsync()
    {
        Logger.LogInformation("Migrating schema for database...");
        await _dbSchemaMigrator.MigrateAsync();
    }

    private async Task SeedDataAsync()
    {
        Logger.LogInformation("Executing database seed...");

        await _dataSeeder.SeedAsync(new DataSeedContext()
            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue)
        );
    }

    private bool AddInitialMigrationIfNotExist()
    {
        try
        {
            if (!DbMigrationsProjectExists())
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        try
        {
            if (!MigrationsFolderExists())
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private bool DbMigrationsProjectExists()
    {
        var dbMigrationsProjectFolder = Path.Combine(
            GetSolutionRootDirectory(),
            "Acme.OnlineCourses"
        );

        return Directory.Exists(dbMigrationsProjectFolder);
    }

    private static bool MigrationsFolderExists()
    {
        var dbMigrationsProjectFolder = Path.Combine(
            GetSolutionRootDirectory(),
            "Acme.OnlineCourses"
        );

        var migrationsFolder = Path.Combine(
            dbMigrationsProjectFolder,
            "Migrations"
        );

        return Directory.Exists(migrationsFolder);
    }

    private static string GetSolutionRootDirectory()
    {
        // Since we will be running this from the project directory, we need to go up one level
        return Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
    }
}
