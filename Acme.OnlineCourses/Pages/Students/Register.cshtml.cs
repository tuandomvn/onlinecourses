using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Volo.Abp.Users;

namespace Acme.OnlineCourses.Pages.Students;

public class RegisterModel : PageModel
{
    private readonly IStudentAppService _studentAppService;
    private readonly IAgencyAppService _agencyAppService;
    private readonly ICourseAppService _courseAppService;
    private readonly ICurrentUser _currentUser;
    private readonly IStringLocalizer<OnlineCoursesResource> _localizer;

    public RegisterModel(
        IStudentAppService studentAppService,
        IAgencyAppService agencyAppService,
        ICourseAppService courseAppService,
        ICurrentUser currentUser,
        IStringLocalizer<OnlineCoursesResource> localizer)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
        _courseAppService = courseAppService;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    [BindProperty]
    public RegisterStudentDto Student { get; set; }

    public List<SelectListItem> Agencies { get; set; }
    public List<SelectListItem> Courses { get; set; }
    public bool IsLoggedIn => _currentUser.IsAuthenticated;

    public async Task<IActionResult> OnGetAsync()
    {
        // If user is logged in, check if they already have a student profile
        // TODO: as is chỉ có 1 khóa nên không cần đang kí nhiều.
        if (_currentUser.IsAuthenticated)
        {
            var existingStudent = await _studentAppService.GetProfileStudentByEmailAsync(_currentUser.Email);
            if (existingStudent != null)
            {
                return RedirectToPage("/Students/Profile");
            }

            Student = new RegisterStudentDto
            {
                Email = _currentUser.Email
            };
        }
        else
        {
            Student = new RegisterStudentDto();
        }

        // Get agencies for dropdown
        var agencies = await _agencyAppService.GetListAsync(new GetAgencyListDto());
        Agencies = new List<SelectListItem>() { new SelectListItem(_localizer["SelectAgency"], "") };
        foreach (var agency in agencies.Items.Where(x => x.Status == AgencyStatus.Active))
        {
            Agencies.Add(new SelectListItem(agency.Name, agency.Id.ToString()));
        }

        var courses = await _courseAppService.GetListAsync();
        Courses = new List<SelectListItem>
        {
            new SelectListItem(_localizer["SelectCourse"], "")
        };
        foreach (var course in courses)
        {
            Courses.Add(new SelectListItem(course.Name, course.Id.ToString()));
        }

        return Page();
    }
}