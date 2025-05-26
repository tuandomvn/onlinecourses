using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

public class CreateUpdateStudentDto : EntityDto<Guid>
{
    [Required]
    [StringLength(128)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    [Phone]
    [StringLength(32)]
    public string PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(32)]
    public string IdentityNumber { get; set; }

    [Required]
    [StringLength(128)]
    public string CourseName { get; set; }

    public DateTime RegistrationDate { get; set; }

    public TestStatus TestStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public AccountStatus AccountStatus { get; set; }

    [StringLength(1024)]
    public string InternalNote { get; set; }

    public Guid? AgencyId { get; set; }

    public Guid? AssignedAdminId { get; set; }

    public string PaymentProofFile { get; set; }
} 