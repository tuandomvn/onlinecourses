using Acme.OnlineCourses.Courses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Acme.OnlineCourses.Students;

public class Student : AuditedAggregateRoot<Guid>
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public Guid? AgencyId { get; set; }
    public string Address { get; set; }
    public bool AgreeToTerms { get; set; }
    public bool IsValid { get; set; }
    public virtual ICollection<StudentAttachment> Attachments { get; set; }
    public List<StudentCourse> Courses { get; set; }

    public Student()
    {
        Attachments = new List<StudentAttachment>();
        Courses = new List<StudentCourse>();
    }

    //public Student(
    //    Guid id,
    //    string fullname,
    //    string email,
    //    string phoneNumber,
    //    DateTime? dateOfBirth,
    //    Guid? agencyId,
    //    string agencyName,
    //    string address,
    //    bool agreeToTerms
    //) : base(id)
    //{
    //    Fullname = fullname;
    //    Email = email;
    //    PhoneNumber = phoneNumber;
    //    DateOfBirth = dateOfBirth;
    //    AgencyId = agencyId;
    //    Address = address;
    //    AgreeToTerms = agreeToTerms;
    //    Courses = new List<StudentCourse>();
    //    Attachments = new List<StudentAttachment>();
    //}
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
    public Course Course { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime ExpectedStudyDate { get; set; }
    public StudentCourseStatus CourseStatus { get; set; }
    public TestStatus TestStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? StudentNote { get; set; }

    public string? AdminNote { get; set; }

    public StudentCourse()
    {
    }

    //public StudentCourse(
    //    Guid id,
    //    Guid studentId,
    //    Guid courseId,
    //    DateTime registrationDate,
    //    DateTime expectedStudyDate,
    //    TestStatus testStatus,
    //    PaymentStatus paymentStatus
    //) : base(id)
    //{
    //    StudentId = studentId;
    //    CourseId = courseId;
    //    RegistrationDate = registrationDate;
    //    ExpectedStudyDate = expectedStudyDate;
    //    TestStatus = testStatus;
    //    PaymentStatus = paymentStatus;
    //}
}

public enum StudentCourseStatus
{
    Inprogress = 0,
    Completed = 1,
    Cancelled = 2
    //OnHold = 3
} 