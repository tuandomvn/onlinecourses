using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Blogs;

[Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
public class IndexModel : PageModel
{
    private readonly IBlogAppService _blogAppService;

    public IndexModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public Language CurrentLanguage { get; set; }

    public async Task OnGetAsync()
    {
        CurrentLanguage = CultureInfo.CurrentCulture.ToLanguage();
        // The page will load blogs via AJAX with current language
    }
} 