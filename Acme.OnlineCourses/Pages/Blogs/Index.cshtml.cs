using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Blogs;

public class IndexModel : PageModel
{
    private readonly IBlogAppService _blogAppService;

    public IndexModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public async Task OnGetAsync()
    {
        // The page will load blogs via AJAX
    }
} 