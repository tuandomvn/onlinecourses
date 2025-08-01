using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination;

namespace Acme.OnlineCourses.Pages.Students;
[Authorize(Roles = OnlineCoursesConsts.Roles.Administrator + "," + OnlineCoursesConsts.Roles.Agency)]
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
        var agencies = (await _agencyAppService.GetListAsync(new GetAgencyListDto())).Items.Where(e => e.Status == AgencyStatus.Active).ToList();
        Agencies = agencies;
    }

    [HttpGet]
    public async Task<JsonResult> OnGetListAsync(GetStudentListDto input)
    {
        var result = await _studentAppService.GetStudentsWithCoursesAsync(input);
        return new JsonResult(result);
    }
}