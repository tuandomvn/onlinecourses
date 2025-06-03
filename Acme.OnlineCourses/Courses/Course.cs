using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses.Courses;

public class Course : Entity<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; } // Duration in hours
    public CourseStatusIN Status { get; set; }
}

public enum CourseStatusIN
{
    Active,
    Inactive,
    ComingSoon
} 

