using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
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
    private readonly IAgencyAppService _agencyAppService;

    public List<AgencyDto> Agencies { get; set; } = new();
    public IndexModel(IStudentAppService studentAppService,
        IAgencyAppService agencyAppService)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
    }

    public async Task OnGetAsync()
    {
        var agencies = (await _agencyAppService.GetListAsync(new GetAgencyListDto())).Items.ToList();
        Agencies = agencies;
    }

    [HttpGet]
    public async Task<JsonResult> OnGetListAsync(GetStudentListDto input)
    {
        var result = await _studentAppService.GetListAsync(input);
        return new JsonResult(result);
    }
}