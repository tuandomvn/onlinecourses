using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Students.Dtos;
using Acme.OnlineCourses.Students;
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
        PagedAndSortedResultRequestDto,
        CreateUpdateAgencyDto,
        CreateUpdateAgencyDto>,
    IAgencyAppService
{
    private readonly IObjectMapper _objectMapper;
    private readonly IRepository<Student, Guid> _studentRepository;
    public AgencyAppService(
        IRepository<Agency, Guid> repository,
        IRepository<Student, Guid> studentRepository,
        IObjectMapper objectMapper)
        : base(repository)
    {
        _objectMapper = objectMapper;
        _studentRepository = studentRepository;
        //GetPolicyName = "AgencyManagement";
        //GetListPolicyName = "AgencyManagement";
        //CreatePolicyName = "AgencyManagement.Create";
        //UpdatePolicyName = "AgencyManagement.Edit";
        //DeletePolicyName = "AgencyManagement.Delete";
    }

    public override async Task<AgencyDto> GetAsync(Guid id)
    {
        var agency = await Repository.GetAsync(id);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    public override async Task<PagedResultDto<AgencyDto>> GetListAsync(PagedAndSortedResultRequestDto input)
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

    protected override async Task<IQueryable<Agency>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);

        if (input is GetAgencyListDto agencyListInput && !string.IsNullOrWhiteSpace(agencyListInput.Filter))
        {
            query = query.Where(x =>
                x.Code.Contains(agencyListInput.Filter) ||
                x.Name.Contains(agencyListInput.Filter) ||
                x.ContactEmail.Contains(agencyListInput.Filter) ||
                x.ContactPhone.Contains(agencyListInput.Filter)
            );
        }

        return query;
    }

    protected override IQueryable<Agency> ApplySorting(IQueryable<Agency> query, PagedAndSortedResultRequestDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            return query.OrderBy(x => x.Name);
        }

        return base.ApplySorting(query, input);
    }


    public async Task<PagedResultDto<StudentDto>> GetStudentsByAgencyAsync(Guid agencyId, PagedAndSortedResultRequestDto input)
    {
        var query = await _studentRepository.GetQueryableAsync();
        //TODO
        //query = query.Where(x => x.AgencyId == agencyId);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<StudentDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Student[], StudentDto[]>(items.ToArray())
        };
    }

    // New method to get students list
    public async Task<PagedResultDto<StudentDto>> GetStudentsListAsync(Guid agencyId, PagedAndSortedResultRequestDto input)
    {
        var query = await _studentRepository.GetQueryableAsync();
        query = query.Where(x => x.AgencyId == agencyId);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync();

        return new PagedResultDto<StudentDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Student[], StudentDto[]>(items.ToArray())
        };
    }

    public async Task<List<StudentDto>> GetStudentsAsync(Guid agencyId)
    {
        var query = await _studentRepository.GetQueryableAsync();
        query = query.Where(x => x.AgencyId == agencyId);

        var totalCount = await query.CountAsync();
        var items = await query
            .ToListAsync();

        return _objectMapper.Map<List<Student>, List<StudentDto>>(items);
    }
} 