using System;
using Acme.OnlineCourses.Agencies;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Students;

public class Student : AuditedAggregateRoot<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string IdentityNumber { get; set; }
    public string CourseName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public TestStatus TestStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public string InternalNote { get; set; }
    public Guid? AgencyId { get; set; }
    public Agency Agency { get; set; }
    public Guid? AssignedAdminId { get; set; }
    public IdentityUser AssignedAdmin { get; set; }
    public string PaymentProofFile { get; set; }
} 