using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses
{
    public class AgentRegister : FullAuditedEntity<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OrganizationName { get; set; }
        public string Position { get; set; }
        public string Message { get; set; }
    }
} 