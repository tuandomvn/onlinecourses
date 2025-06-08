using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Blogs;

public interface IBlogAppService :
    ICrudAppService<
        BlogDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateBlogDto,
        CreateUpdateBlogDto>
{
    Task<List<BlogDto>> GetPublishedBlogsAsync();
    Task<BlogDto> GetByCodeAsync(string code, Language? language = null);
} 