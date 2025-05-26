using Acme.OnlineCourses.Agencies.Dtos;
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
} 