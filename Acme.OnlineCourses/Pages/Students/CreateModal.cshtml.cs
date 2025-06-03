using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Pages.Students;

public class CreateModalModel : OnlineCoursesPageModel
{
    [BindProperty]
    public CreateUpdateStudentDto Student { get; set; }

    public List<SelectListItem> TestStatusList { get; set; }
    public List<SelectListItem> PaymentStatusList { get; set; }
    public List<SelectListItem> AccountStatusList { get; set; }
    public List<SelectListItem> AgencyList { get; set; }

    private readonly IStudentAppService _studentAppService;
    private readonly IAgencyAppService _agencyAppService;
    private readonly IStringLocalizer<OnlineCoursesResource> _localizer;

    public CreateModalModel(
        IStudentAppService studentAppService,
        IAgencyAppService agencyAppService,
        IStringLocalizer<OnlineCoursesResource> localizer)
    {
        _studentAppService = studentAppService;
        _agencyAppService = agencyAppService;
        _localizer = localizer;
    }

    public async Task OnGetAsync()
    {
        Student = new CreateUpdateStudentDto
        {
            TestStatus = TestStatus.NotTaken,
            PaymentStatus = PaymentStatus.NotPaid,
            AccountStatus = AccountStatus.NotSent
        };

        await LoadSelectListsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _studentAppService.CreateAsync(Student);
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
                Text = _localizer[$"Enum:TestStatus:{x}"]
            })
            .ToList();

        // Load payment status list
        PaymentStatusList = Enum.GetValues(typeof(PaymentStatus))
            .Cast<PaymentStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = _localizer[$"Enum:PaymentStatus:{x}"]
            })
            .ToList();

        // Load account status list
        AccountStatusList = Enum.GetValues(typeof(AccountStatus))
            .Cast<AccountStatus>()
            .Select(x => new SelectListItem
            {
                Value = ((int)x).ToString(),
                Text = _localizer[$"Enum:AccountStatus:{x}"]
            })
            .ToList();

        // Load agency list
        var agencies = await _agencyAppService.GetListAsync(new PagedAndSortedResultRequestDto
        {
            MaxResultCount = 1000
        });

        AgencyList = agencies.Items
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToList();
    }
} 