using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.OnlineCourses.Controllers;

[RemoteService]
[Route("api/app/blog")]
public class BlogController : AbpController
{
    private readonly IBlogAppService _blogAppService;

    public BlogController(IBlogAppService blogAppService)
    {
        _blogAppService = blogAppService;
    }

    [HttpGet]
    [Route("by-code/{code}")]
    public async Task<BlogDto> GetByCodeAsync(string code, [FromQuery] Language? language = null)
    {
        return await _blogAppService.GetByCodeAsync(code, language);
    }
} 