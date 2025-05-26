using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Acme.OnlineCourses.Pages.Blogs;

public class CreateModalModel : PageModel
{
    [BindProperty]
    public CreateUpdateBlogDto Blog { get; set; }

    private readonly IBlogAppService _blogAppService;

    public CreateModalModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public void OnGet()
    {
        Blog = new CreateUpdateBlogDto();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _blogAppService.CreateAsync(Blog);
        return new OkResult();
    }
} 