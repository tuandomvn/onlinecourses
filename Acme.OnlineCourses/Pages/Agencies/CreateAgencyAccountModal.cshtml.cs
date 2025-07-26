using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Helpers;
using Acme.OnlineCourses.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using static Acme.OnlineCourses.OnlineCoursesConsts;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Acme.OnlineCourses.Pages.Agencies
{
    [Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
    public class CreateAgencyAccountModal : PageModel
    {
        [BindProperty]
        public Guid AgencyId { get; set; }

        [BindProperty]
        public string AgencyName { get; set; }

        [BindProperty]
        public bool IsAccountProvided { get; set; }

        [BindProperty]
        public CreateAccountDto AccountInfo { get; set; }

        private readonly IAgencyAppService _agencyAppService;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IStringLocalizer<OnlineCoursesResource> _localizer;
        private readonly IMailService _mailService;

        public CreateAgencyAccountModal(
            IAgencyAppService agencyAppService,
            IIdentityUserRepository userRepository,
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IMailService mailService,
            IStringLocalizer<OnlineCoursesResource> localizer)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _agencyAppService = agencyAppService;
            _localizer = localizer;
            _mailService = mailService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (AccountInfo == null)
            {
                AccountInfo = new CreateAccountDto();
            }

            var agency = await _agencyAppService.GetAsync(id);
            AgencyId = agency.Id;
            AgencyName = agency.Name;
            AccountInfo.Email = agency.ContactEmail;
            AccountInfo.PhoneNumber = agency.ContactPhone;
            IsAccountProvided = await IsAgencyExistsAsync(agency.ContactEmail);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var isAccountProvided = await IsAgencyExistsAsync(AccountInfo.Email);
                if (!isAccountProvided)
                {
                    var agencyUser = new IdentityUser(
                       Guid.NewGuid(),
                       AccountInfo.Email, AccountInfo.Email);

                    agencyUser.SetProperty("AgencyId", AgencyId);

                    var password = PasswordGenerator.GenerateSecurePassword(8);
                    await _userManager.CreateAsync(agencyUser, password);
                    await _userManager.AddToRoleAsync(agencyUser, "agency");
                    // Send welcome email
                    _mailService.SendWelcomePartnerEmailAsync(new WelcomeRequest
                    {
                        ToEmail = AccountInfo.Email,
                        UserName = AccountInfo.Email,
                        Password = password
                    });
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }

        public async Task<bool> IsAgencyExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var normalizedEmail = email.ToUpperInvariant();
            var user = await _userRepository.FindByNormalizedUserNameAsync(normalizedEmail);

            if (user == null)
            {
                return false;
            }

            // Kiểm tra xem user có role Agency hay không
            var userRoles = await _userRepository.GetRoleNamesAsync(user.Id);
            return userRoles.Contains(Roles.Agency);
        }

        

    }

    public class CreateAccountDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}