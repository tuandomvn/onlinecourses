using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Agencies
{
    public class StudentsModalModel : PageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid AgencyId { get; set; }

        private readonly IAgencyAppService _agencyAppService;

        public StudentsModalModel(IAgencyAppService agencyAppService)
        {
            _agencyAppService = agencyAppService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetStudentsAsync()
        {
            var students = await _agencyAppService.GetStudentsByAgencyAsync(
                AgencyId,
                new PagedAndSortedResultRequestDto
                {
                    MaxResultCount = 1000
                }
            );

            return new JsonResult(students);
        }
    }
} 