using System;
using System.Threading.Tasks;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Pages.Agencies
{
    [Authorize(Roles = OnlineCoursesConsts.Roles.Agency)]
    public class ProfileModel : PageModel
    {
        private readonly IAgencyAppService _agencyAppService;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IRepository<Acme.OnlineCourses.Agencies.Agency, Guid> _agencyRepository;

        [BindProperty]
        public CreateUpdateAgencyDto Agency { get; set; }

        public ProfileModel(
            IAgencyAppService agencyAppService,
            ICurrentUser currentUser,
            IIdentityUserRepository userRepository,
            IRepository<Acme.OnlineCourses.Agencies.Agency, Guid> agencyRepository)
        {
            _agencyAppService = agencyAppService;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _agencyRepository = agencyRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var agencyId = await _currentUser.GetAgencyIdAsync(_userRepository, _agencyRepository);
            if (!agencyId.HasValue)
            {
                return NotFound();
            }

            var dto = await _agencyAppService.GetAsync(agencyId.Value);
            Agency = new CreateUpdateAgencyDto
            {
                Id = dto.Id,
                Name = dto.Name,
                OrgName = dto.OrgName,
                Description = dto.Description,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                Address = dto.Address,
                Status = dto.Status,
                IsAccountProvided = false
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var agencyId = await _currentUser.GetAgencyIdAsync(_userRepository, _agencyRepository);
            if (!agencyId.HasValue)
            {
                return NotFound();
            }

            var existing = await _agencyAppService.GetAsync(agencyId.Value);
            Agency.Id = agencyId.Value;
            Agency.ContactEmail = existing.ContactEmail;
            Agency.CommissionPercent = existing.CommissionPercent;
            Agency.Code = existing.Code;
            Agency.CityCode = existing.CityCode;

            await _agencyAppService.UpdateAsync(agencyId.Value, Agency);
            return RedirectToPage();
        }
    }
}

