using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Blogs
{
    public class ShowModel : PageModel
    {
        private readonly IBlogAppService _blogAppService;

        public BlogDto Blog { get; set; }

        public ShowModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var input = new GetBlogBySlugInput { Slug = slug };
            Blog = await _blogAppService.GetBySlugAsync(input);

            if (Blog == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 