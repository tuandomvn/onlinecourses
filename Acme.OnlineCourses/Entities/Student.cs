using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Acme.OnlineCourses.Students;

public class Student : AuditedAggregateRoot<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string IdentityNumber { get; set; }
    public TestStatus TestStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public Guid? AgencyId { get; set; }
    public string Address { get; set; }
    public bool AgreeToTerms { get; set; }
    public string? StudentNote { get; set; }
    public string? AdminNote { get; set; }
    public bool IsValid { get; set; }
    public virtual ICollection<StudentAttachment> Attachments { get; set; }
    public List<StudentCourse> Courses { get; set; }

    public Student()
    {
        Attachments = new List<StudentAttachment>();
        Courses = new List<StudentCourse>();
    }

    public Student(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        DateTime? dateOfBirth,
        string identityNumber,
        TestStatus testStatus,
        PaymentStatus paymentStatus,
        AccountStatus accountStatus,
        Guid? agencyId,
        string agencyName,
        string address,
        bool agreeToTerms
    ) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        IdentityNumber = identityNumber;
        TestStatus = testStatus;
        PaymentStatus = paymentStatus;
        AccountStatus = accountStatus;
        AgencyId = agencyId;
        Address = address;
        AgreeToTerms = agreeToTerms;
        Courses = new List<StudentCourse>();
        Attachments = new List<StudentAttachment>();
    }
}

public class StudentAttachment : Entity<Guid>
{
    public Guid StudentId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }
    public string Description { get; set; }
    public StudentAttachment()
    {
    }

    public StudentAttachment(
        Guid id,
        Guid studentId,
        string fileName,
        string filePath,
        string description
    ) : base(id)
    {
        StudentId = studentId;
        FileName = fileName;
        FilePath = filePath;
        UploadDate = DateTime.Now;
        Description = description;
    }
}

public class StudentCourse : Entity<Guid>
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string CourseNote { get; set; }
    public StudentCourseStatus CourseStatus { get; set; }

    public StudentCourse()
    {
    }

    public StudentCourse(
        Guid id,
        Guid studentId,
        Guid courseId,
        DateTime registrationDate
    ) : base(id)
    {
        StudentId = studentId;
        CourseId = courseId;
        RegistrationDate = registrationDate;
    }
}

public enum StudentCourseStatus
{
    Active = 0,
    Completed = 1,
    Cancelled = 2,
    OnHold = 3
} 