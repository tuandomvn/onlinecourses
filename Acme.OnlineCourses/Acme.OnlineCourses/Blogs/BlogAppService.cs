using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Blogs;

public class BlogAppService :
    CrudAppService<
        Blog,
        BlogDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateBlogDto,
        CreateUpdateBlogDto>,
    IBlogAppService
{
    public BlogAppService(IRepository<Blog, Guid> repository)
        : base(repository)
    {
        //GetPolicyName = OnlineCoursesPermissions.Blogs.Default;
        //GetListPolicyName = OnlineCoursesPermissions.Blogs.Default;
        //CreatePolicyName = OnlineCoursesPermissions.Blogs.Create;
        //UpdatePolicyName = OnlineCoursesPermissions.Blogs.Edit;
        //DeletePolicyName = OnlineCoursesPermissions.Blogs.Delete;
    }

    public async Task<List<BlogDto>> GetPublishedBlogsAsync()
    {
        var query = await Repository.GetQueryableAsync();
        var blogs = await query
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.PublishedDate)
            .ToListAsync();

        return ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs);
    }

    protected override async Task<IQueryable<Blog>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        var query = await Repository.GetQueryableAsync();
        if (input is GetBlogListDto blogListInput && !string.IsNullOrWhiteSpace(blogListInput.Filter))
        {
            query = query.Where(x =>
                x.Title.Contains(blogListInput.Filter) ||
                x.Summary.Contains(blogListInput.Filter)
            );
        }
        return query;
    }

    protected override IQueryable<Blog> ApplySorting(IQueryable<Blog> query, PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            return query.OrderByDescending(x => x.CreationTime);
        }

        return base.ApplySorting(query, input);
    }
} 