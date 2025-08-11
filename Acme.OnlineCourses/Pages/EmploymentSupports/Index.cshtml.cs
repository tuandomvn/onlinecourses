using Acme.OnlineCourses.EmpSupport;
using Acme.OnlineCourses.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using static Acme.OnlineCourses.OnlineCoursesConsts;

namespace Acme.OnlineCourses.Pages.EmploymentSupports
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<EmploymentSupport, Guid> _employmentSupportRepository;
        private readonly IMailService _mailService;
        private readonly IdentityUserManager _userManager;
        public IndexModel(IRepository<EmploymentSupport, Guid> employmentSupportRepository, IMailService mailService, IdentityUserManager userManager)
        {
            _mailService = mailService;
            _userManager = userManager;
            _employmentSupportRepository = employmentSupportRepository;
        }

        [BindProperty]
        public EmploymentSupportInput EmploymentSupport { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var entity = new EmploymentSupport
            {
                FullName = EmploymentSupport.FullName,
                DateOfBirth = EmploymentSupport.DateOfBirth,
                PhoneNumber = EmploymentSupport.PhoneNumber,
                Email = EmploymentSupport.Email,
                Address = EmploymentSupport.Address,
                CourseCompletionDate = EmploymentSupport.CourseCompletionDate,
                Message = EmploymentSupport.Message ?? string.Empty
            };
            await _employmentSupportRepository.InsertAsync(entity, autoSave: true);
            TempData["FormMessage"] = "success";

            var adminEmails = await GetAdminEmailsAsync();

            _mailService.SendJobNotiToAdminsAsync(new NotityNewPartnerToAdminRequest
            {
                ToEmail = adminEmails,
                Name = entity.FullName,
                Email = entity.Email
            });

            return RedirectToPage();
        }
        private async Task<List<string>> GetAdminEmailsAsync()
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(Roles.Administrator);
            var _cachedAdminEmails = adminUsers.Select(u => u.Email).ToList();

            return _cachedAdminEmails;
        }
        public class EmploymentSupportInput
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

            public DateTime DateOfBirth { get; set; }

            public string PhoneNumber { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            public string Address { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime CourseCompletionDate { get; set; }

            public string? Message { get; set; }
        }
    }
}