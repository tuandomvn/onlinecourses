using Acme.OnlineCourses.Agencies.Dtos;
using AutoMapper;

namespace Acme.OnlineCourses.Agencies;

public class AgencyAutoMapperProfile : Profile
{
    public AgencyAutoMapperProfile()
    {
        CreateMap<Agency, AgencyDto>()
            .ForMember(dest => dest.AgencyCode, opt => opt.MapFrom(src => src.Code));
        CreateMap<CreateUpdateAgencyDto, Agency>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgencyCode))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifierId, opt => opt.Ignore())
            //.ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            //.ForMember(dest => dest.DeleterId, opt => opt.Ignore())
            //.ForMember(dest => dest.DeletionTime, opt => opt.Ignore())
            ;
    }
} 