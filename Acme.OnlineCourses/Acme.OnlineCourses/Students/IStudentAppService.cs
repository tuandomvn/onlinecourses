using System;
using System.Threading.Tasks;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Students;

public interface IStudentAppService :
    ICrudAppService<
        StudentDto,
        Guid,
        GetStudentListDto,
        CreateUpdateStudentDto>
{
    Task<StudentDto> RegisterStudentAsync(RegisterStudentDto input);
} 