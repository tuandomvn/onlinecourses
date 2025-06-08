using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.OnlineCourses.Students.Dtos;

public class RegisterStudentDto
{
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

    [Required]
    [StringLength(512)]
    public string Address { get; set; }

    public string StudentNote { get; set; }

    public Guid? AgencyId { get; set; }
    public Guid? CourseId { get; set; } //Co the bo di

    [Required]
    public bool AgreeToTerms { get; set; }
} 