using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Blogs
{
    [Authorize]
    public class ShowModel : PageModel
    {
        private readonly IBlogAppService _blogAppService;

        public BlogDto Blog { get; set; }
        public Language CurrentLanguage { get; set; }

        public ShowModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public async Task OnGetAsync(string slug)
        {
            CurrentLanguage = CultureInfo.CurrentCulture.ToLanguage();
            Blog = await _blogAppService.GetByCodeAsync(slug, CurrentLanguage);
        }
    }
} 