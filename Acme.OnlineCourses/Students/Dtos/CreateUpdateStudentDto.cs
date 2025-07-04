using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

//Admin site
public class CreateUpdateStudentDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(128)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(128)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    [Required]
    [Phone]
    [StringLength(32)]
    public string PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required]
    [StringLength(32)]
    public string IdentityNumber { get; set; }

    public TestStatus TestStatus { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public AccountStatus AccountStatus { get; set; }

    public Guid? AgencyId { get; set; }

    [Required]
    [StringLength(512)]
    public string Address { get; set; }

    [Required]
    public bool AgreeToTerms { get; set; }

    public List<CreateUpdateStudentAttachmentDto> Attachments { get; set; }
}

public class CreateUpdateStudentAttachmentDto
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string Description { get; set; }
} 