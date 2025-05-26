using System;
using Acme.OnlineCourses.Agencies.Dtos;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

public class StudentDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
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
    public string AgencyName { get; set; }
    public Guid? AssignedAdminId { get; set; }
    public string AssignedAdminName { get; set; }
    public string PaymentProofFile { get; set; }
} 