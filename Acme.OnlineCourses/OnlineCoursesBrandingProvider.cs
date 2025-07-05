using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Acme.OnlineCourses.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Acme.OnlineCourses;

[Dependency(ReplaceServices = true)]
public class OnlineCoursesBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<OnlineCoursesResource> _localizer;
    private ILogger<OnlineCoursesBrandingProvider> _logger;

    public OnlineCoursesBrandingProvider(IStringLocalizer<OnlineCoursesResource> localizer, ILogger<OnlineCoursesBrandingProvider> logger)
    {
        _localizer = localizer;
        _logger = logger;
    }

    public override string AppName
    {
        get
        {
            try
            {
                var appName = _localizer["AppName"];
                _logger.LogInformation("Localized AppName: {AppName}", appName);
                return appName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get localized AppName");
                // Fallback to default name if localization fails
                return "Online Courses";
            }
        }
    }
}
