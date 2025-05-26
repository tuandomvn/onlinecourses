using Acme.OnlineCourses.Agencies.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Acme.OnlineCourses.Agencies;

public class AgencyAutoMapperProfile : Profile
{
    public AgencyAutoMapperProfile()
    {
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