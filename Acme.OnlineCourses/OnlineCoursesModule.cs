using Acme.OnlineCourses.Data;
using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Menus;
using Acme.OnlineCourses.ScriptContributors;
using Acme.OnlineCourses.Students;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using DataSeeder = Acme.OnlineCourses.Data.DataSeeder;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Acme.OnlineCourses;

[DependsOn(
    // ABP Framework packages
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),

    // Account module packages
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),

    // Identity module packages
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpIdentityWebModule),

    // Audit logging module packages
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),

    // Tenant Management module packages
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementWebModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpFeatureManagementWebModule),

    // Setting Management module packages
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSettingManagementWebModule)
)]
public class OnlineCoursesModule : AbpModule
{
    /* Single point to enable/disable multi-tenancy */
    public const bool IsMultiTenant = false;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(OnlineCoursesResource)
            );
        });

        // Simple OpenIddict configuration without certificates
        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("OnlineCourses");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        // Add server configuration with ephemeral keys to avoid certificate issues
        PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
        {
            serverBuilder.AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey()
                        .DisableAccessTokenEncryption();
        });

        // Disable certificate requirements for OpenIddict
        PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
        {
            options.AddDevelopmentEncryptionAndSigningCertificate = false;
        });

        OnlineCoursesGlobalFeatureConfigurator.Configure();
        OnlineCoursesModuleExtensionConfigurator.Configure();
        OnlineCoursesEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }

        ConfigureAuthentication(context);
        ConfigureMultiTenancy();
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureAutoMapper(context);
        ConfigureSwagger(context.Services);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureVirtualFiles(hostingEnvironment);
        ConfigureLocalization();
        ConfigureEfCore(context);

        // Đăng ký DataSeeder
        context.Services.AddTransient<IDataSeedContributor, DataSeeder>();
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        // Use standard Identity authentication instead of OpenIddict
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = IsMultiTenant;
        });
    }


    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                //LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                StandardBundles.Scripts.Global,
                bundle => { bundle.AddContributors(typeof(GlobalScriptContributor)); }
            );
        });

    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<OnlineCoursesResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/OnlineCourses");

            options.DefaultResourceType = typeof(OnlineCoursesResource);

            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("vi", "vi", "Tiếng Việt"));
        
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("OnlineCourses", typeof(OnlineCoursesResource));
        });
    }

    private void ConfigureVirtualFiles(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OnlineCoursesModule>();
            // Use physical files in both development and production to ensure localization works
            options.FileSets.ReplaceEmbeddedByPhysical<OnlineCoursesModule>(hostingEnvironment.ContentRootPath);
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new OnlineCoursesMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(OnlineCoursesModule).Assembly);
        });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineCourses API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<OnlineCoursesModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            /* Uncomment `validate: true` if you want to enable the Configuration Validation feature.
             * See AutoMapper's documentation to learn what it is:
             * https://docs.automapper.org/en/stable/Configuration-validation.html
             */
            options.AddMaps<OnlineCoursesModule>(/* validate: true */);
        });
    }

    private void ConfigureEfCore(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OnlineCoursesDbContext>(options =>
        {
            /* You can remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots
             * Documentation: https://docs.abp.io/en/abp/latest/Entity-Framework-Core#add-default-repositories
             */
            options.AddDefaultRepositories(includeAllEntities: true);

            // Add custom repositories
            options.AddRepository<StudentCourse, EfCoreRepository<OnlineCoursesDbContext, StudentCourse, Guid>>();
            options.AddRepository<StudentAttachment, EfCoreRepository<OnlineCoursesDbContext, StudentAttachment, Guid>>();
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseMySQL();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (IsMultiTenant)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineCourses API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        // Chạy DataSeeder khi ứng dụng khởi động
        var seeder = context.ServiceProvider.GetRequiredService<IDataSeedContributor>();
        seeder.SeedAsync(new DataSeedContext()).GetAwaiter().GetResult();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        // Data seeding is typically done during initialization, not shutdown
        // context.GetEnvironment().GetService<IDataSeeder>().SeedAsync();
    }
}
