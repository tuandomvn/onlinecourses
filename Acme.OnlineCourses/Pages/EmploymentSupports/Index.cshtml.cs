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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entity = new EmploymentSupport
            {
                FullName = EmploymentSupport.FullName,
                DateOfBirth = EmploymentSupport.DateOfBirth,
                PhoneNumber = EmploymentSupport.PhoneNumber,
                Email = EmploymentSupport.Email,
                Address = EmploymentSupport.Address,
                CourseCompletionDate = EmploymentSupport.CourseCompletionDate,
                Message = EmploymentSupport.Message
            };
            await _employmentSupportRepository.InsertAsync(entity, autoSave: true);
            TempData["SuccessMessage"] = "Đã gửi thông tin hỗ trợ việc làm thành công!";
            return RedirectToPage();
        }

        public class EmploymentSupportInput
        {
            [Required]
            [Display(Name = "Họ và tên")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Ngày sinh")]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }

            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "Ngày hoàn thành khóa học")]
            [DataType(DataType.Date)]
            public DateTime CourseCompletionDate { get; set; }

            [Display(Name = "Tin nhắn")]
            public string Message { get; set; }
        }
    }
} 