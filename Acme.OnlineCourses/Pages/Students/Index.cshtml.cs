using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Acme.OnlineCourses.Pages.Students;

public class IndexModel : PageModel
{
    private readonly IStudentAppService _studentAppService;

    public IndexModel(IStudentAppService studentAppService)
    {
        _studentAppService = studentAppService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    [HttpGet]
    public async Task<JsonResult> OnGetListAsync(GetStudentListDto input)
    {
        var result = await _studentAppService.GetListAsync(input);
        return new JsonResult(result);
    }
}