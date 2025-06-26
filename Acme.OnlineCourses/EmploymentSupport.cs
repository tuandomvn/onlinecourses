using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses
{
    public class EmploymentSupport : Volo.Abp.Domain.Entities.Auditing.FullAuditedEntity<System.Guid>
    {
        public string FullName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public System.DateTime CourseCompletionDate { get; set; }
        public string Message { get; set; }
    }
} 