using AutoMapper;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Entities;

namespace Acme.OnlineCourses.Agencies;

public class AgencyAutoMapperProfile : Profile
{
    public AgencyAutoMapperProfile()
    {
        CreateMap<Agency, AgencyDto>();
        CreateMap<CreateUpdateAgencyDto, Agency>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreatorId, opt => opt.Ignore())
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifierId, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeleterId, opt => opt.Ignore())
            .ForMember(dest => dest.DeletionTime, opt => opt.Ignore());
    }
} 