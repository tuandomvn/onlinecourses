using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages;

public class AboutModel : AbpPageModel
{
    private readonly IBlogAppService _blogAppService;

    public BlogDto AboutBlog { get; set; }

    public AboutModel(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    public async Task OnGetAsync()
    {
        AboutBlog = await _blogAppService.GetByCodeAsync("BLG002");
    }
} 