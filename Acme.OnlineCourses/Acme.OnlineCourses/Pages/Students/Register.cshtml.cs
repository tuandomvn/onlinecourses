using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acme.OnlineCourses.Pages.Students;

[Authorize(Roles = "student")]
public class RegisterModel : PageModel
{
    private readonly IStudentAppService _studentAppService;
    private readonly IAgencyAppService _agencyAppService;

    public RegisterModel(
        IStudentAppService studentAppService,
        IAgencyAppService agencyAppService)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
    }

    [BindProperty]
    public RegisterStudentDto Student { get; set; }

    public List<SelectListItem> Agencies { get; set; }

    public async Task OnGetAsync()
    {
        var agencies = await _agencyAppService.GetListAsync(new GetAgencyListDto { MaxResultCount = 1000 });
        Agencies = new List<SelectListItem>
        {
            new SelectListItem("-- Select Agency --", "")
        };

        foreach (var agency in agencies.Items)
        {
            Agencies.Add(new SelectListItem(agency.Name, agency.Id.ToString()));
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _studentAppService.RegisterStudentAsync(Student);
        return RedirectToPage("./Index");
    }
} 