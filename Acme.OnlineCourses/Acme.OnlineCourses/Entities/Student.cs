using System;
using System.Collections.Generic;
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
    public string InternalNote { get; set; }
    public Guid? AgencyId { get; set; }
    public string AgencyName { get; set; }
    public string Address { get; set; }
    public bool AgreeToTerms { get; set; }
    public List<StudentAttachment> Attachments { get; set; }
    public List<StudentCourse> Courses { get; set; }
    public CourseStatus CourseStatus { get; set; }
    public string CourseNote { get; set; }
    public Guid? AssignedAdminId { get; set; }
    public IdentityUser AssignedAdmin { get; set; }
    public string PaymentProofFile { get; set; }

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
        string internalNote,
        Guid? agencyId,
        string agencyName,
        string address,
        bool agreeToTerms,
        CourseStatus courseStatus = CourseStatus.Active,
        string courseNote = null,
        Guid? assignedAdminId = null,
        string paymentProofFile = null
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
        InternalNote = internalNote;
        AgencyId = agencyId;
        AgencyName = agencyName;
        Address = address;
        AgreeToTerms = agreeToTerms;
        CourseStatus = courseStatus;
        CourseNote = courseNote;
        AssignedAdminId = assignedAdminId;
        PaymentProofFile = paymentProofFile;
        Attachments = new List<StudentAttachment>();
        Courses = new List<StudentCourse>();
    }
}

public class StudentAttachment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }

    public StudentAttachment()
    {
    }

    public StudentAttachment(
        Guid id,
        Guid studentId,
        string fileName,
        string filePath
    )
    {
        Id = id;
        StudentId = studentId;
        FileName = fileName;
        FilePath = filePath;
        UploadDate = DateTime.Now;
    }
}

public class StudentCourse : AuditedEntity<Guid>
{
    public Guid StudentId { get; set; }
    public string CourseName { get; set; }
    public DateTime RegistrationDate { get; set; }

    protected StudentCourse()
    {
    }

    public StudentCourse(
        Guid id,
        Guid studentId,
        string courseName,
        DateTime registrationDate
    ) : base(id)
    {
        StudentId = studentId;
        CourseName = courseName;
        RegistrationDate = registrationDate;
    }
}

public enum CourseStatus
{
    Active = 0,
    Completed = 1,
    Cancelled = 2,
    OnHold = 3
} 