using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Acme.OnlineCourses;
using System.Linq;

namespace Acme.OnlineCourses.Pages.EmploymentSupportAdmin
{
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