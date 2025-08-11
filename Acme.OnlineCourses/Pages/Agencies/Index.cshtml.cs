using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acme.OnlineCourses.Pages.Agencies
{
    [Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
} 