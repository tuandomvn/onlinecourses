using AutoMapper;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;

namespace Acme.OnlineCourses.Students;

public class StudentAutoMapperProfile : Profile
{
    public StudentAutoMapperProfile()
    {
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
    }
} 