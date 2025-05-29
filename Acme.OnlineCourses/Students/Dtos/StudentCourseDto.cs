using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

public class StudentCourseDto : AuditedEntityDto<Guid>
{
    public Guid StudentId { get; set; }
    public string CourseName { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string CourseNote { get; set; }

}