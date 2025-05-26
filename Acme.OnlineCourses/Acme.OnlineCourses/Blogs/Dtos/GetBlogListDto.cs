using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Blogs.Dtos;

public class GetBlogListDto : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
} 