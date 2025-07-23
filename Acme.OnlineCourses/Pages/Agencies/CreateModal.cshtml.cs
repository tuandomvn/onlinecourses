using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public List<SelectListItem> Locations { get; set; } // Add this property

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
            Locations = GetLocationsList(); // Populate Locations
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

        // Add this method to provide the city list
        private List<SelectListItem> GetLocationsList()
        {
            var isEnglish = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en");
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "hanoi", Text = "Hà Nội" },
                new SelectListItem { Value = "hue", Text = "Huế" },
                new SelectListItem { Value = "danang", Text = "Đà Nẵng" },
                new SelectListItem { Value = "haiphong", Text = "Hải Phòng" },
                new SelectListItem { Value = "cantho", Text = "Cần Thơ" },
                new SelectListItem { Value = "hochiminh", Text = "Hồ Chí Minh" },
                new SelectListItem { Value = "angiang", Text = "An Giang" },
                new SelectListItem { Value = "bacninh", Text = "Bắc Ninh" },
                new SelectListItem { Value = "camau", Text = "Cà Mau" },
                new SelectListItem { Value = "caobang", Text = "Cao Bằng" },
                new SelectListItem { Value = "daklak", Text = "Đắk Lắk" },
                new SelectListItem { Value = "dienbien", Text = "Điện Biên" },
                new SelectListItem { Value = "dongnai", Text = "Đồng Nai" },
                new SelectListItem { Value = "dongthap", Text = "Đồng Tháp" },
                new SelectListItem { Value = "gialai", Text = "Gia Lai" },
                new SelectListItem { Value = "hatinh", Text = "Hà Tĩnh" },
                new SelectListItem { Value = "hungyen", Text = "Hưng Yên" },
                new SelectListItem { Value = "khanhhoa", Text = "Khánh Hòa" },
                new SelectListItem { Value = "laichau", Text = "Lai Châu" },
                new SelectListItem { Value = "lamdong", Text = "Lâm Đồng" },
                new SelectListItem { Value = "langson", Text = "Lạng Sơn" },
                new SelectListItem { Value = "laocai", Text = "Lào Cai" },
                new SelectListItem { Value = "nghean", Text = "Nghệ An" },
                new SelectListItem { Value = "ninhbinh", Text = "Ninh Bình" },
                new SelectListItem { Value = "phutho", Text = "Phú Thọ" },
                new SelectListItem { Value = "quangngai", Text = "Quảng Ngãi" },
                new SelectListItem { Value = "quangninh", Text = "Quảng Ninh" },
                new SelectListItem { Value = "quangtri", Text = "Quảng Trị" },
                new SelectListItem { Value = "sonla", Text = "Sơn La" },
                new SelectListItem { Value = "tayninh", Text = "Tây Ninh" },
                new SelectListItem { Value = "thainguyen", Text = "Thái Nguyên" },
                new SelectListItem { Value = "thanhhoa", Text = "Thanh Hóa" },
                new SelectListItem { Value = "tuyenquang", Text = "Tuyên Quang" },
                new SelectListItem { Value = "vinhlong", Text = "Vĩnh Long" }
            }.OrderBy(e => e.Value).ToList();
        }
    }
}