using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Agencies.Dtos;

public class CreateUpdateAgencyDto : EntityDto<Guid>
{
    [Required]
    [StringLength(32)]
    public string Code { get; set; }

    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    [StringLength(256)]
    public string OrgName { get; set; }

    [StringLength(1024)]
    public string Description { get; set; }

    [EmailAddress]
    [StringLength(256)]
    public string ContactEmail { get; set; }

    [Phone]
    [StringLength(32)]
    public string ContactPhone { get; set; }

    [StringLength(512)]
    public string Address { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal CommissionPercent { get; set; }
    
    public string? CityCode { get; set; }

    public AgencyStatus Status { get; set; }
} 