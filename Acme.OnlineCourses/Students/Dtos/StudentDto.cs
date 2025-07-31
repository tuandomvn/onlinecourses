using Acme.OnlineCourses.Agencies.Dtos;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.OnlineCourses.Students.Dtos;

//DTO này dung cho student view only
public class ProfileStudentDto : StudentDto
{
}

//DTO này dung cho admin view
public class AdminViewStudentDto : StudentDto
{
    public string? AdminNote { get; set; }
    // Thêm các field từ StudentCourse để hiển thị trên index
    public string? CourseName { get; set; }
    public Guid? CourseId { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public StudentCourseStatus? CourseStatus { get; set; }
    public TestStatus? TestStatus { get; set; }
    public PaymentStatus? CoursePaymentStatus { get; set; }
    public string? CourseNote { get; set; }
    // Thêm role của user hiện tại
    public string? CurrentUserRole { get; set; }
}

//DTO mới cho việc cập nhật StudentCourse
public class UpdateStudentCourseDto
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public StudentCourseStatus CourseStatus { get; set; }
    public TestStatus TestStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    [TextArea(Rows = 4)]
    public string? StudentNote { get; set; }

    [TextArea(Rows = 4)]
    public string? AdminNote { get; set; }

    public List<StudentAttachment> Attachments { get; set; } = [];
}

//Share info
public class StudentDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public Guid? AgencyId { get; set; }
    public string Address { get; set; }
    public bool AgreeToTerms { get; set; }
    public string? StudentNote { get; set; }
    public List<StudentAttachmentDto> Attachments { get; set; }
    public List<StudentCourseDto> Courses { get; set; }

    // Thêm các field từ StudentCourse để hiển thị trên index
    [Obsolete]
    public DateTime? RegistrationDate { get; set; }
    [Obsolete]
    public StudentCourseStatus? CourseStatus { get; set; }
    [Obsolete]
    public TestStatus? TestStatus { get; set; }
    [Obsolete]
    public string? CourseNote { get; set; }
}

public class StudentAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }
    public string Description { get; set; }
}
