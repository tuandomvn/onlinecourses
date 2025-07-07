using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Agencies.Dtos;

public class AgencyDto : AuditedEntityDto<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public string? CityCode { get; set; }
    public decimal CommissionPercent { get; set; }
    public AgencyStatus Status { get; set; }
}

public class GetAgencyListDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}

public class GetStudentFromAgencyDto : PagedAndSortedResultRequestDto
{
    public Guid AgencyId { get; set; }
}