using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Acme.OnlineCourses.Pages.Blogs;

public class EditModalModel : PageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateBlogDto Blog { get; set; }

    private readonly IBlogAppService _blogAppService;
    private readonly IObjectMapper _objectMapper;

    public EditModalModel(
        IBlogAppService blogAppService,
        IObjectMapper objectMapper)
    {
        _blogAppService = blogAppService;
        _objectMapper = objectMapper;
    }

    public async Task OnGetAsync()
    {
        var blog = await _blogAppService.GetAsync(Id);
        Blog = _objectMapper.Map<BlogDto, CreateUpdateBlogDto>(blog);
        Blog.Id = Id;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Blog.Id != Id)
        {
            Blog.Id = Id;
        }
        await _blogAppService.UpdateAsync(Id, Blog);
        return new OkResult();
    }
} 