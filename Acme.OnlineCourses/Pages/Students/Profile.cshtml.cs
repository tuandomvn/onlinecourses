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
using Volo.Abp.Users;

namespace Acme.OnlineCourses.Pages.Students;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IStudentAppService _studentAppService;
    private readonly IAgencyAppService _agencyAppService;
    private readonly ICurrentUser _currentUser;

    public ProfileModel(
        IStudentAppService studentAppService,
        IAgencyAppService agencyAppService,
        ICurrentUser currentUser)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
        _currentUser = currentUser;
    }

    [BindProperty]
    public StudentDto Student { get; set; }

    public List<SelectListItem> Agencies { get; set; }

    public List<StudentAttachmentDto> Attachments { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_currentUser.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login");
        }

        // Get student by email
        var student = await _studentAppService.GetByEmailAsync(_currentUser.Email);
        if (student == null)
        {
            return RedirectToPage("/Students/Register");
        }

        Student = student;

        // Get agencies for dropdown
        var agencies = await _agencyAppService.GetListAsync(new GetAgencyListDto());
        Agencies = new List<SelectListItem>();
        foreach (var agency in agencies.Items)
        {
            Agencies.Add(new SelectListItem(agency.Name, agency.Id.ToString()));
        }

        // Get attachments
        Attachments = await _studentAppService.GetAttachmentsAsync(student.Id);

        return Page();
    }
} 