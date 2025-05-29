using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages;

public class IndexModel : AbpPageModel
{
    private readonly IBlogAppService _blogAppService;

    public BlogDto FeaturedBlog { get; set; }

    public IndexModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public async Task OnGetAsync()
    {
        FeaturedBlog = await _blogAppService.GetByCodeAsync("BLG001");
    }
}