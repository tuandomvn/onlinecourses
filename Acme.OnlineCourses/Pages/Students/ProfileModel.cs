using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Users;

namespace Acme.OnlineCourses.Web.Pages.Students
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IStudentAppService _studentAppService;
        private readonly ICurrentUser _currentUser;
        [BindProperty]
        public StudentDto Student { get; set; }

        public ProfileModel(IStudentAppService studentAppService, ICurrentUser currentUser)
        {
            _studentAppService = studentAppService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var email = _currentUser.Email;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Account/Login");
            }

            Student = await _studentAppService.GetByEmailAsync(email);
            if (Student == null)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
} 