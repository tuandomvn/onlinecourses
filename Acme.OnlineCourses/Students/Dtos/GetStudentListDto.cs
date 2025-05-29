using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Students.Dtos;

public class GetStudentListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public Guid? AgencyId { get; set; }
    public TestStatus? TestStatus { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public AccountStatus? AccountStatus { get; set; }
    public CourseStatus? CourseStatus { get; set; }
} 