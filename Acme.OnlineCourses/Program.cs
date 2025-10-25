using Acme.OnlineCourses.Data;
using Acme.OnlineCourses.Helpers;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Acme.OnlineCourses;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var loggerConfiguration = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()

            // Log of (Acme.OnlineCourses) in file application.txt
            .WriteTo.Logger(lc => lc.Filter
                .ByIncludingOnly(evt =>evt.Properties.ContainsKey("SourceContext") &&
                   evt.Properties["SourceContext"].ToString().Contains("Acme.OnlineCourses")
                )
                .WriteTo.Async(c => c.File("Logs/application.txt", rollingInterval: RollingInterval.Day,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"))
                )

            // Log of ABP in file framework.txt
            //.WriteTo.Logger(lc => lc.Filter
            //    .ByExcluding(evt => evt.Properties.ContainsKey("SourceContext") && evt.Properties["SourceContext"].ToString().Contains("Acme.OnlineCourses"))
            //    .WriteTo.Async(c => c.File( "Logs/framework.txt", rollingInterval: RollingInterval.Day,
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")))
            //        .WriteTo.Async(c => c.Console())
            ;

        if (IsMigrateDatabase(args))
        {
            loggerConfiguration.MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning);
            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            if (IsMigrateDatabase(args))
            {
                builder.Services.AddDataMigrationEnvironment();
            }

            // Đăng ký cấu hình MailSettings từ appsettings.json
            builder.Services.Configure<MailSettings>(
                builder.Configuration.GetSection("MailSettings"));

            // Đăng ký MailService với DI container
            builder.Services.AddScoped<IMailService, Helpers.MailService>();

            // Cấu hình session timeout là 8 giờ
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(8); // Thời gian timeout là 8 giờ
                options.SlidingExpiration = true; // Gia hạn session khi user có hoạt động
                options.Cookie.Name = "OnlineCourses.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            await builder.AddApplicationAsync<OnlineCoursesModule>();
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            await app.InitializeApplicationAsync();

            if (IsMigrateDatabase(args))
            {
                await app.Services.GetRequiredService<OnlineCoursesDbMigrationService>().MigrateAsync();
                return 0;
            }

            Log.Information("Starting Acme.OnlineCourses.");
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Acme.OnlineCourses terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static bool IsMigrateDatabase(string[] args)
    {
        return args.Any(x => x.Contains("--migrate-database", StringComparison.OrdinalIgnoreCase));
    }
}
