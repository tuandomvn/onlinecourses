using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.OnlineCourses.Pages.Agencies
{
    public class CreateModalModel : PageModel
    {
        [BindProperty]
        public CreateUpdateAgencyDto Agency { get; set; }

        public List<SelectListItem> StatusList { get; set; }

        private readonly IAgencyAppService _agencyAppService;
        private readonly IStringLocalizer<OnlineCoursesResource> _localizer;

        public CreateModalModel(
            IAgencyAppService agencyAppService,
            IStringLocalizer<OnlineCoursesResource> localizer)
        {
            _agencyAppService = agencyAppService;
            _localizer = localizer;
        }

        public void OnGet()
        {
            Agency = new CreateUpdateAgencyDto();
            StatusList = GetStatusList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _agencyAppService.CreateAsync(Agency);
            return new OkResult();
        }

        private List<SelectListItem> GetStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = AgencyStatus.Active.ToString(), Text = _localizer["Enum:AgencyStatus:Active"] },
                new SelectListItem { Value = AgencyStatus.Inactive.ToString(), Text = _localizer["Enum:AgencyStatus:Inactive"] }
            };
        }
    }
} 