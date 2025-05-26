using System;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Students;

public interface IStudentAppService : 
    ICrudAppService<
        StudentDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateStudentDto,
        CreateUpdateStudentDto>
{
} 