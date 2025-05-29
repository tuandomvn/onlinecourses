using Microsoft.Extensions.Localization;
using Acme.OnlineCourses.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Acme.OnlineCourses;

[Dependency(ReplaceServices = true)]
public class OnlineCoursesBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<OnlineCoursesResource> _localizer;

    public OnlineCoursesBrandingProvider(IStringLocalizer<OnlineCoursesResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
