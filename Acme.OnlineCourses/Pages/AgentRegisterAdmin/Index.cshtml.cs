using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Acme.OnlineCourses;
using System.Linq;

namespace Acme.OnlineCourses.Pages.AgentRegisterAdmin
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<AgentRegister, Guid> _agentRegisterRepository;

        public IndexModel(IRepository<AgentRegister, Guid> agentRegisterRepository)
        {
            _agentRegisterRepository = agentRegisterRepository;
        }

        public IList<AgentRegister> AgentRegisters { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;
        
        public int TotalCount { get; set; }
        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public async Task OnGetAsync()
        {
            TotalCount = (int)await _agentRegisterRepository.GetCountAsync();
            
            AgentRegisters = await _agentRegisterRepository.GetPagedListAsync(
                skipCount: (CurrentPage - 1) * PageSize,
                maxResultCount: PageSize,
                sorting: "CreationTime DESC"
            );
        }
    }
} 