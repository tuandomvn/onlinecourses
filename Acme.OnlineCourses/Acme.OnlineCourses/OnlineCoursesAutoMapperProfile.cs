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

        CreateMap<Student, StudentDto>();
        CreateMap<CreateUpdateStudentDto, Student>();
        CreateMap<StudentDto, CreateUpdateStudentDto>();

        CreateMap<StudentCourse, StudentCourseDto>();
        CreateMap<CreateUpdateStudentCourseDto, StudentCourse>();

        CreateMap<StudentAttachment, StudentAttachmentDto>();
        CreateMap<CreateUpdateStudentAttachmentDto, StudentAttachment>();

        CreateMap<Blog, BlogDto>();
        CreateMap<CreateUpdateBlogDto, Blog>();
        CreateMap<BlogDto, CreateUpdateBlogDto>();

        CreateMap<Agency, AgencyDto>();
        CreateMap<CreateUpdateAgencyDto, Agency>();
        CreateMap<AgencyDto, CreateUpdateAgencyDto>();
    }
} 