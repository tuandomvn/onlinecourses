using System;
using Volo.Abp.Domain.Entities.Auditing;
using Acme.OnlineCourses.Agencies;

namespace Acme.OnlineCourses.Entities;

public class Agency : FullAuditedEntity<Guid>
{
    public string AgencyCode { get; set; } = string.Empty; // Mã trung tâm
    public string Name { get; set; } = string.Empty; // Tên trung tâm
    public string ContactEmail { get; set; } = string.Empty; // Email liên hệ
    public string ContactPhone { get; set; } = string.Empty; // SĐT
    public double CommissionPercent { get; set; } // Phần trăm hoa hồng
    public AgencyStatus Status { get; set; } // Trạng thái
} 