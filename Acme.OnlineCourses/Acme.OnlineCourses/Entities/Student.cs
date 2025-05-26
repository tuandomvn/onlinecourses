using System;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Students;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Entities;

public class Student : FullAuditedEntity<Guid>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? IdentityNumber { get; set; } // CCCD
    public string CourseName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public TestStatus TestStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public string? InternalNote { get; set; }
    
    public Guid AgencyId { get; set; }
    public Agency Agency { get; set; }
    
    public Guid? AssignedAdminId { get; set; }
    public IdentityUser AssignedAdmin { get; set; }
    
    public string? PaymentProofFile { get; set; } // Path to payment proof file
} 