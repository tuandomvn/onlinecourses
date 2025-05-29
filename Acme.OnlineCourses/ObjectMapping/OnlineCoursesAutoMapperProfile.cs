using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Students.Dtos;
using Acme.OnlineCourses.Students;
using AutoMapper;

namespace Acme.OnlineCourses.ObjectMapping;

public class OnlineCoursesAutoMapperProfile : Profile
{
    public OnlineCoursesAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */

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
