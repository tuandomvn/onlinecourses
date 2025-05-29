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
using Volo.Abp.ObjectMapping;

namespace Acme.OnlineCourses.Pages.Agencies
{
    public class EditModalModel : PageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateAgencyDto Agency { get; set; }

        public List<SelectListItem> StatusList { get; set; }

        private readonly IAgencyAppService _agencyAppService;
        private readonly IStringLocalizer<OnlineCoursesResource> _localizer;
        private readonly IObjectMapper _objectMapper;

        public EditModalModel(
            IAgencyAppService agencyAppService,
            IStringLocalizer<OnlineCoursesResource> localizer,
            IObjectMapper objectMapper)
        {
            _agencyAppService = agencyAppService;
            _localizer = localizer;
            _objectMapper = objectMapper;
        }

        public async Task OnGetAsync()
        {
            var agency = await _agencyAppService.GetAsync(Id);
            Agency = _objectMapper.Map<AgencyDto, CreateUpdateAgencyDto>(agency);
            StatusList = GetStatusList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _agencyAppService.UpdateAsync(Id, Agency);
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