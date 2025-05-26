using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace Acme.OnlineCourses.Pages;

public abstract class OnlineCoursesPageModel : PageModel
{
    public IClock Clock { get; set; }
    public IObjectMapper ObjectMapper { get; set; }
} 