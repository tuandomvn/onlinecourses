using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class CreateUpdateBlogDto : EntityDto<Guid>
{
    [Required]
    [StringLength(256)]
    public string Title { get; set; }

    [StringLength(1024)]
    public string Summary { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime? PublishedDate { get; set; }

    public bool IsPublished { get; set; }
} 