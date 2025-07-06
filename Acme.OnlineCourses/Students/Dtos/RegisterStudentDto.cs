using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.OnlineCourses.Students.Dtos;

public class RegisterStudentDto
{
    [Required]
    [StringLength(128)]
    public string Fullname { get; set; }

    //[Required]
    //[StringLength(128)]
    //public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    [Required]
    [Phone]
    [StringLength(32)]
    public string PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public DateTime? ExpectedStudyDate { get; set; }

    [Required]
    [StringLength(512)]
    public string Address { get; set; }

    public string? StudentNote { get; set; }

    public Guid? AgencyId { get; set; }

    [Required]
    public bool AgreeToTerms { get; set; }
} 

public class UpdateStudentDto 
{
    public Guid Id { get; set; }
    [StringLength(128)]
    public string? FirstName { get; set; }

    [StringLength(128)]
    public string? LastName { get; set; }

    [EmailAddress]
    [StringLength(256)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(32)]
    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(512)]
    public string? Address { get; set; }

    public string? StudentNote { get; set; }

    public Guid? AgencyId { get; set; }

    [Required]
    public bool AgreeToTerms { get; set; }
}