using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Acme.OnlineCourses;

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

        public async Task OnGetAsync()
        {
            EmploymentSupports = await _employmentSupportRepository.GetListAsync();
        }
    }
} 