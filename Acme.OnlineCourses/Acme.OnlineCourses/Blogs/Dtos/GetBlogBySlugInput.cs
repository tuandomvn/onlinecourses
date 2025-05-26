using System.ComponentModel.DataAnnotations;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class GetBlogBySlugInput
{
    [Required]
    public string Slug { get; set; }
} 