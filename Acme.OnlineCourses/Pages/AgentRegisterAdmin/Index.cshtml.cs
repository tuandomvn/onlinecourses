using Acme.OnlineCourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Pages.AgentRegisterAdmin
{
    [Authorize]
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