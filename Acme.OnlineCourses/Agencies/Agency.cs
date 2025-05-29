using System;
using System.Collections.Generic;
using Acme.OnlineCourses.Students;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses.Agencies;

public class Agency : AuditedAggregateRoot<Guid>
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public decimal CommissionPercent { get; set; }
    public AgencyStatus Status { get; set; }
    public ICollection<Student> Students { get; set; }
}

public enum AgencyStatus
{
    Active,
    Inactive,
    Suspended
} 