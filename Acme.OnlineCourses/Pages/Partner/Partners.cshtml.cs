using Acme.OnlineCourses.Agencies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Pages.Partner;

public class PartnerModel : AbpPageModel
{
    private readonly IRepository<Agency, Guid> _agencytRepository;
    public PartnerModel(IRepository<Agency, Guid> agencytRepository)
    {
        _agencytRepository = agencytRepository;
    }

    public List<Agency> Agencies { get; set; }
    public List<SelectListItem> Locations { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SelectedLocationId { get; set; }

    [BindProperty]
    public PartnerApplicationViewModel PartnerApplication { get; set; }

    public async Task OnGetAsync()
    {
        var isEnglish = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en");

        Locations = new List<SelectListItem>
        {
            new SelectListItem { Value = "all", Text = isEnglish ? "All" : "Tất cả", Selected = true },
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

        if (string.IsNullOrEmpty(SelectedLocationId))
        {
            Agencies = (await _agencytRepository.GetListAsync()).Where(x => x.Status == AgencyStatus.Active).ToList();
        }
        else
        {
            if (SelectedLocationId == "all")
            {
                Agencies = (await _agencytRepository.GetListAsync()).Where(x => x.Status == AgencyStatus.Active).ToList();
            }
            else
            {
                Agencies = (await _agencytRepository.GetListAsync())
                    .Where(x => x.Status == AgencyStatus.Active && x.CityCode == SelectedLocationId)
                    .ToList();
            }

            var selectedItem = Locations.Find(l => l.Value == SelectedLocationId);
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
            }
        }
    }

    public async Task<JsonResult> OnPostRegisterAsync([FromBody] PartnerRegistrationRequest request)
    {
        try
        {
            var agency = new Agency
            {
                Name = request.PartnerApplication.FullName,
                Description = request.PartnerApplication.Message,
                ContactEmail = request.PartnerApplication.Email,
                ContactPhone = request.PartnerApplication.PhoneNumber,
                Address = request.PartnerApplication.Address,
                Status = AgencyStatus.Inactive, // Set as pending until approved
                CommissionPercent = 0,
                Code = GenerateAgencyCode(request.PartnerApplication.OrganizationName),
                CityCode = request.PartnerApplication.CityCode,
                OrgName = request.PartnerApplication.OrganizationName,
            };

            await _agencytRepository.InsertAsync(agency);

            return new JsonResult(new { success = true });
        }
        catch (System.Exception ex)
        {
            return new JsonResult(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    private string GenerateAgencyCode(string organizationName)
    {
        // Generate a unique code for the agency based on the organization name
        return $"{organizationName.Substring(0, Math.Min(5, organizationName.Length)).ToUpper()}-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }
}

public class PartnerApplicationViewModel
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    public string OrganizationName { get; set; }
    public string Position { get; set; }
    public string Message { get; set; }
    [Required]
    public string? CityCode { get; set; } // Optional, can be null
}

public class PartnerRegistrationRequest
{
    public PartnerApplicationViewModel PartnerApplication { get; set; }
}