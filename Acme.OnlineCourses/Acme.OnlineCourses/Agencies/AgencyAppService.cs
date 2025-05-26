using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Data;
using Acme.OnlineCourses.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Acme.OnlineCourses.Agencies;

public class AgencyAppService : 
    CrudAppService<
        Agency,
        AgencyDto,
        Guid,
        GetAgencyListDto,
        CreateUpdateAgencyDto,
        CreateUpdateAgencyDto>,
    IAgencyAppService
{
    private readonly IObjectMapper _objectMapper;

    public AgencyAppService(
        IRepository<Agency, Guid> repository,
        IObjectMapper objectMapper)
        : base(repository)
    {
        _objectMapper = objectMapper;
        GetPolicyName = "AgencyManagement";
        GetListPolicyName = "AgencyManagement";
        CreatePolicyName = "AgencyManagement.Create";
        UpdatePolicyName = "AgencyManagement.Edit";
        DeletePolicyName = "AgencyManagement.Delete";
    }

    public override async Task<AgencyDto> GetAsync(Guid id)
    {
        var agency = await Repository.GetAsync(id);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    public override async Task<PagedResultDto<AgencyDto>> GetListAsync(GetAgencyListDto input)
    {
        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await query.CountAsync();
        var items = await query
            //.OrderBy(input.Sorting ?? nameof(Agency.Name))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<AgencyDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Agency[], AgencyDto[]>(items.ToArray())
        };
    }

    public override async Task<AgencyDto> CreateAsync(CreateUpdateAgencyDto input)
    {
        var agency = _objectMapper.Map<CreateUpdateAgencyDto, Agency>(input);
        agency = await Repository.InsertAsync(agency);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    public override async Task<AgencyDto> UpdateAsync(Guid id, CreateUpdateAgencyDto input)
    {
        var agency = await Repository.GetAsync(id);
        _objectMapper.Map(input, agency);
        agency = await Repository.UpdateAsync(agency);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    protected override async Task<IQueryable<Agency>> CreateFilteredQueryAsync(GetAgencyListDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.AgencyCode.Contains(input.Filter) ||
                x.Name.Contains(input.Filter) ||
                x.ContactEmail.Contains(input.Filter) ||
                x.ContactPhone.Contains(input.Filter)
            );
        }

        return query;
    }

    protected override IQueryable<Agency> ApplySorting(IQueryable<Agency> query, GetAgencyListDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            return query.OrderBy(x => x.Name);
        }

        return base.ApplySorting(query, input);
    }
} 