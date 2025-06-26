using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Pages.AgentRegisters
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<AgentRegister, Guid> _agentRegisterRepository;

        public CreateModel(IRepository<AgentRegister, Guid> agentRegisterRepository)
        {
            _agentRegisterRepository = agentRegisterRepository;
        }

        [BindProperty]
        public AgentRegisterInput AgentRegister { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entity = new AgentRegister
            {
                FullName = AgentRegister.FullName,
                PhoneNumber = AgentRegister.PhoneNumber,
                Email = AgentRegister.Email,
                Address = AgentRegister.Address,
                OrganizationName = AgentRegister.OrganizationName,
                Position = AgentRegister.Position,
                Message = AgentRegister.Message
            };
            await _agentRegisterRepository.InsertAsync(entity, autoSave: true);
            return RedirectToPage("/AgentRegisterAdmin/Index");
        }

        public class AgentRegisterInput
        {
            [Required]
            public string FullName { get; set; }
            [Required]
            public string PhoneNumber { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Address { get; set; }
            public string OrganizationName { get; set; }
            public string Position { get; set; }
            public string Message { get; set; }
        }
    }
} 