using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class CreateUpdateBlogDto : EntityDto<Guid>
{
    [Required]
    [StringLength(256)]
    public string Title { get; set; }

    [Required]
    [StringLength(256)]
    public string Slug { get; set; }

    [StringLength(1024)]
    public string Summary { get; set; }

    [Required]
    public string Content { get; set; }

    [StringLength(1024)]
    public string FeaturedImage { get; set; }

    [StringLength(128)]
    public string Author { get; set; }

    public DateTime? PublishedDate { get; set; }

    public bool IsPublished { get; set; }

    [StringLength(512)]
    public string Tags { get; set; }
} 