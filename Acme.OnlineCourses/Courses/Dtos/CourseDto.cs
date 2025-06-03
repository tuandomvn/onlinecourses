using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Courses.Dtos;

public class CourseDto : EntityDto<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public CourseStatusIN Status { get; set; }
} 