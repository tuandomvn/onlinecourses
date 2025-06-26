using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Acme.OnlineCourses;

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

        public async Task OnGetAsync()
        {
            AgentRegisters = await _agentRegisterRepository.GetListAsync();
        }
    }
} 