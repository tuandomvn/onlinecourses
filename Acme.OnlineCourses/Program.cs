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
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console());

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

            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.ExpireTimeSpan = TimeSpan.FromHours(10); // 10 giờ
            //    options.SlidingExpiration = true; // Tự động gia hạn nếu còn hoạt động
            //});
            
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromHours(10);
            });

            await builder.AddApplicationAsync<OnlineCoursesModule>();
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

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
