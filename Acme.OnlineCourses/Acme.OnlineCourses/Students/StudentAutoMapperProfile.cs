using AutoMapper;
using Acme.OnlineCourses.Entities;
using Acme.OnlineCourses.Students.Dtos;

namespace Acme.OnlineCourses.Students;

public class StudentAutoMapperProfile : Profile
{
    public StudentAutoMapperProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.AssignedAdminName, opt => opt.MapFrom(src => src.AssignedAdmin != null ? src.AssignedAdmin.UserName : null))
            .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Agency));

        CreateMap<CreateUpdateStudentDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifierId, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeleterId, opt => opt.Ignore())
            .ForMember(dest => dest.DeletionTime, opt => opt.Ignore())
            .ForMember(dest => dest.Agency, opt => opt.Ignore())
            .ForMember(dest => dest.AssignedAdmin, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth ?? DateTime.MinValue));

        CreateMap<StudentDto, CreateUpdateStudentDto>();
    }
} 