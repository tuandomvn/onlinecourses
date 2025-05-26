using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Agencies.Dtos;

public class AgencyDto : AuditedEntityDto<Guid>
{
    public string AgencyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public double CommissionPercent { get; set; }
    public AgencyStatus Status { get; set; }
}

public class CreateUpdateAgencyDto
{
    public string AgencyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public double CommissionPercent { get; set; }
    public AgencyStatus Status { get; set; }
}

public class GetAgencyListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
} 