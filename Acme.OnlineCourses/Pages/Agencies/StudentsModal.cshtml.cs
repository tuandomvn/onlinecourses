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

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid AgencyId { get; set; }

        public StudentsModalModel(IAgencyAppService agencyAppService)
        {
            _agencyAppService = agencyAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        //public async Task<JsonResult> OnGetStudentsAsync(
        //    [FromQuery] Guid agencyId,
        //    [FromQuery] int skipCount = 0,
        //    [FromQuery] int maxResultCount = 10,
        //    [FromQuery] string sorting = null)
        //{
        //    var input = new GetStudentListDto
        //    {
        //        AgencyId = agencyId,
        //        SkipCount = skipCount,
        //        MaxResultCount = maxResultCount
        //    };

        //    var result = await _agencyAppService.GetStudentsAsync(input);
        //    return new JsonResult(new
        //    {
        //        draw = 1,
        //        recordsTotal = result.TotalCount,
        //        recordsFiltered = result.TotalCount,
        //        data = result.Items
        //    });
        //}
    }
} 