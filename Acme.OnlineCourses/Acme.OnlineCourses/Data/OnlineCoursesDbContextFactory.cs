using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Acme.OnlineCourses.Data;

public class OnlineCoursesDbContextFactory : IDesignTimeDbContextFactory<OnlineCoursesDbContext>
{
    public OnlineCoursesDbContext CreateDbContext(string[] args)
    {
        OnlineCoursesEfCoreEntityExtensionMappings.Configure();
        
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OnlineCoursesDbContext>()
            .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

        return new OnlineCoursesDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
