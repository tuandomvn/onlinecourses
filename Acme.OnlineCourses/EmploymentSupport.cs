using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses
{
    public class EmploymentSupport : FullAuditedEntity<Guid>
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CourseCompletionDate { get; set; }
        public string Message { get; set; }
    }
} 