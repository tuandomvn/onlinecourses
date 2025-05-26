using System;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class BlogDto : AuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }
    public string FeaturedImage { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string Author { get; set; }
    public string Tags { get; set; }
} 