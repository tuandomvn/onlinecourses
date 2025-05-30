using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Students.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Agencies;

public interface IAgencyAppService : 
    ICrudAppService<
        AgencyDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAgencyDto,
        CreateUpdateAgencyDto>
{
    Task<PagedResultDto<StudentDto>> GetStudentsByAgencyAsync(Guid agencyId, PagedAndSortedResultRequestDto input);
    Task<PagedResultDto<StudentDto>> GetStudentsListAsync(GetStudentFromAgencyDto input);
    Task<PagedResultDto<StudentDto>> GetStudentsAsync(GetStudentListDto dto);
} 