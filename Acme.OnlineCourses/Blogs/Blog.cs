using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.OnlineCourses.Blogs;

public class Blog : AuditedAggregateRoot<Guid>
{
    public string Code { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }
} 