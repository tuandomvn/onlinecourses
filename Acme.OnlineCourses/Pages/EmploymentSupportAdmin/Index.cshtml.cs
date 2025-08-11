using Acme.OnlineCourses.EmpSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Pages.EmploymentSupportAdmin
{
    [Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
    public class IndexModel : PageModel
    {
        private readonly IRepository<EmploymentSupport, Guid> _employmentSupportRepository;

        public IndexModel(IRepository<EmploymentSupport, Guid> employmentSupportRepository)
        {
            _employmentSupportRepository = employmentSupportRepository;
        }

        public IList<EmploymentSupport> EmploymentSupports { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;
        
        public int TotalCount { get; set; }
        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public async Task OnGetAsync()
        {
            TotalCount = (int)await _employmentSupportRepository.GetCountAsync();
            
            EmploymentSupports = await _employmentSupportRepository.GetPagedListAsync(
                skipCount: (CurrentPage - 1) * PageSize,
                maxResultCount: PageSize,
                sorting: "CreationTime DESC"
            );
        }
    }
} 