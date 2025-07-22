using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Acme.OnlineCourses;

namespace Acme.OnlineCourses.Pages.EmploymentSupports
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<EmploymentSupport, Guid> _employmentSupportRepository;

        public IndexModel(IRepository<EmploymentSupport, Guid> employmentSupportRepository)
        {
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
            return RedirectToPage();
        }

        public class EmploymentSupportInput
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            //[DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            public string PhoneNumber { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            public string Address { get; set; }

            [Required]
           // [DataType(DataType.Date)]
            public DateTime CourseCompletionDate { get; set; }

            public string? Message { get; set; }
        }
    }
} 