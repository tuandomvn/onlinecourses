using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Agencies;
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

        //CreateMap<Student, StudentDto>();
        //CreateMap<CreateUpdateStudentDto, Student>();

        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .Ignore(dest => dest.AgencyName)
            .Ignore(dest => dest.AssignedAdminName);

        CreateMap<CreateUpdateStudentDto, Student>()
            .Ignore(dest => dest.Agency)
            .Ignore(dest => dest.AssignedAdmin)
            .Ignore(dest => dest.ExtraProperties)
            .Ignore(dest => dest.ConcurrencyStamp)
            .Ignore(dest => dest.LastModificationTime)
            .Ignore(dest => dest.LastModifierId)
            .Ignore(dest => dest.CreationTime)
            .Ignore(dest => dest.CreatorId);

        CreateMap<StudentDto, CreateUpdateStudentDto>();

        CreateMap<Blog, BlogDto>();
        CreateMap<CreateUpdateBlogDto, Blog>()
            .Ignore(x => x.LastModificationTime)
            .Ignore(x => x.LastModifierId)
            .Ignore(x => x.CreationTime)
            .Ignore(x => x.CreatorId)
            .Ignore(x => x.ExtraProperties)
            .Ignore(x => x.ConcurrencyStamp);
        CreateMap<BlogDto, CreateUpdateBlogDto>()
            ;

        CreateMap<Agency, AgencyDto>();

        CreateMap<CreateUpdateAgencyDto, Agency>()
            .Ignore(dest => dest.Students)
            .Ignore(dest => dest.ExtraProperties)
            .Ignore(dest => dest.ConcurrencyStamp)
            .Ignore(dest => dest.LastModificationTime)
            .Ignore(dest => dest.LastModifierId)
            .Ignore(dest => dest.CreationTime)
            .Ignore(dest => dest.CreatorId);

        CreateMap<AgencyDto, CreateUpdateAgencyDto>();
    }
} 