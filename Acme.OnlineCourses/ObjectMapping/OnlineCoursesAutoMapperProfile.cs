using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs.Dtos;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Students.Dtos;
using Acme.OnlineCourses.Students;
using AutoMapper;
using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses.Courses.Dtos;

namespace Acme.OnlineCourses.ObjectMapping;

public class OnlineCoursesAutoMapperProfile : Profile
{
    public OnlineCoursesAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */

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

        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<Course, CourseDto>();
    }
}
