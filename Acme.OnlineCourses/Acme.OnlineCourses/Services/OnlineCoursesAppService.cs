using Acme.OnlineCourses.Localization;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Services;

/* Inherit your application services from this class. */
public abstract class OnlineCoursesAppService : ApplicationService
{
    protected OnlineCoursesAppService()
    {
        LocalizationResource = typeof(OnlineCoursesResource);
    }
}