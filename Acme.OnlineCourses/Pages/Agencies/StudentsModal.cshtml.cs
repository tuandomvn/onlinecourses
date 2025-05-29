using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Agencies
{
    //[Authorize(OnlineCoursesPermissions.Agencies.Default)]
    public class StudentsModalModel : PageModel
    {
        private readonly IAgencyAppService _agencyAppService;

        public List<StudentDto> Students { get; set; }

        public StudentsModalModel(IAgencyAppService agencyAppService)
        {
            _agencyAppService = agencyAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid agencyId)
        {
            var input = new GetStudentListDto
            {
                AgencyId = agencyId,
                MaxResultCount = 1000 // Load tất cả học viên của agency
            };

            var result = await _agencyAppService.GetStudentsAsync(input.AgencyId.Value);
            Students = result;

            return Page();
        }
    }
} 