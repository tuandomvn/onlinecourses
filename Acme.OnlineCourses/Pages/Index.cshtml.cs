using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages;

public class IndexModel : AbpPageModel
{
    private readonly IBlogAppService _blogAppService;

    public BlogDto FeaturedBlog { get; set; }
    public Language CurrentLanguage { get; set; }

    public IndexModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public async Task OnGetAsync()
    {
        CurrentLanguage = CultureInfo.CurrentCulture.ToLanguage();
        FeaturedBlog = await _blogAppService.GetByCodeAsync("BLG001", CurrentLanguage);
    }
}