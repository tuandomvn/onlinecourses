using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Acme.OnlineCourses;

public class OnlineCoursesAutoMapperProfile : Profile
{
    public OnlineCoursesAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Student, StudentDto>();
        CreateMap<CreateUpdateStudentDto, Student>();

        CreateMap<Blog, BlogDto>();
        CreateMap<CreateUpdateBlogDto, Blog>()
            .Ignore(x => x.LastModificationTime)
            .Ignore(x => x.LastModifierId)
            .Ignore(x => x.CreationTime)
            .Ignore(x => x.CreatorId)
            .Ignore(x => x.ExtraProperties)
            .Ignore(x => x.ConcurrencyStamp);
    }
} 