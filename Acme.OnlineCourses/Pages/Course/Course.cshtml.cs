using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages.Course;

public class CourseModel : AbpPageModel
{
    public async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}
