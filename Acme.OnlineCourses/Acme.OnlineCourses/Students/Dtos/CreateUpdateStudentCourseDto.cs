using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.OnlineCourses.Students.Dtos;

public class CreateUpdateStudentCourseDto
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    [StringLength(256)]
    public string CourseName { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; }
} 