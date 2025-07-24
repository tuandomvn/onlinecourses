using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Pages.Students;
[Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
public class EditModalModel : OnlineCoursesPageModel
{
    [BindProperty]
    public UpdateStudentCourseDto StudentCourse { get; set; }

    public List<SelectListItem> TestStatusList { get; set; }
    public List<SelectListItem> PaymentStatusList { get; set; }
    public List<SelectListItem> CourseStatusList { get; set; }

    private readonly IStudentAppService _studentAppService;
    private readonly IAgencyAppService _agencyAppService;
    private readonly IIdentityUserAppService _identityUserAppService;
    private readonly IStringLocalizer<OnlineCoursesResource> _localizer;

    public EditModalModel(
        IStudentAppService studentAppService,
        IAgencyAppService agencyAppService,
        IIdentityUserAppService identityUserAppService,
        IStringLocalizer<OnlineCoursesResource> localizer)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
        _identityUserAppService = identityUserAppService;
        _localizer = localizer;
    }

    public async Task OnGetAsync(Guid studentId, Guid courseId)
    {
        StudentCourse = await _studentAppService.GetStudentCourseAsync(studentId, courseId);
        await LoadSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _studentAppService.UpdateStudentCourseAsync(StudentCourse);
        return new OkResult();
    }

    private async Task LoadSelectListsAsync()
    {
        // Load test status list
        TestStatusList = Enum.GetValues(typeof(TestStatus))
            .Cast<TestStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = _localizer[$"Enum:TestStatus:{(int)x}"]
            })
            .ToList();

        // Load payment status list
        PaymentStatusList = Enum.GetValues(typeof(PaymentStatus))
            .Cast<PaymentStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = _localizer[$"Enum:PaymentStatus:{(int)x}"]
            })
            .ToList();

        // Load course status list
        CourseStatusList = Enum.GetValues(typeof(StudentCourseStatus))
            .Cast<StudentCourseStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = _localizer[$"Enum:StudentCourseStatus:{(int)x}"]
            })
            .ToList();
    }
} 