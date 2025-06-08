using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class BlogDto : AuditedEntityDto<Guid>
{
    public string Code { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public DateTime? PublishedDate { get; set; }
    public bool IsPublished { get; set; }
    public Language Language { get; set; }
} 