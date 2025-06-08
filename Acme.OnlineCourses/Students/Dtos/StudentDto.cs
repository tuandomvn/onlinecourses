using System;
using System.Collections.Generic;
using Acme.OnlineCourses.Agencies.Dtos;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

//DTO này dung cho student view only
public class ProfileStudentDto : StudentDto
{
}

//DTO này dung cho admin view
public class AdminViewStudentDto : StudentDto
{
    public string? AdminNote { get; set; }
}


//Share info
public class StudentDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string IdentityNumber { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public Guid? AgencyId { get; set; }
    public string Address { get; set; }
    public bool AgreeToTerms { get; set; }
    public string? StudentNote { get; set; }
    public List<StudentAttachmentDto> Attachments { get; set; }
    public List<StudentCourseDto> Courses { get; set; }
}

public class StudentAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadDate { get; set; }
    public string Description { get; set; }
}
