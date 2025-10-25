using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;
    public AgencyAppService(
        IRepository<Agency, Guid> repository,
        IRepository<Student, Guid> studentRepository,
        ILogger<AgencyAppService> logger,
        IObjectMapper objectMapper)
        : base(repository)
    {
        _objectMapper = objectMapper;
        _logger = logger;
        _studentRepository = studentRepository;
        GetPolicyName = OnlineCoursesPermissions.Agencies.Default;
        GetListPolicyName = OnlineCoursesPermissions.Agencies.Default;
        CreatePolicyName = OnlineCoursesPermissions.Agencies.Create;
        UpdatePolicyName = OnlineCoursesPermissions.Agencies.Edit;
        DeletePolicyName = OnlineCoursesPermissions.Agencies.Delete;
    }

    public override async Task<AgencyDto> GetAsync(Guid id)
    {
        var agency = await Repository.GetAsync(id);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    public override async Task<PagedResultDto<AgencyDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        _logger.LogInformation($"Getting list of agencies with input: {input.MaxResultCount}");

        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .OrderBy(e => e.OrgName)
            .ToListAsync();

        return new PagedResultDto<AgencyDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Agency[], AgencyDto[]>(items.ToArray())
        };
    }

    public async Task<PagedResultDto<AgencyDto>> GetListAllAgencyAsync(PagedAndSortedResultRequestDto input)
    {
        _logger.LogInformation($"Getting list GetListAllAgencyAsync with input: {input.MaxResultCount}");

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

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .OrderByDescending(e => e.CreationTime)
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
        // Map all properties except Id
        agency.Code = input.Code;
        agency.Name = input.Name;
        agency.Description = input.Description;
        agency.ContactEmail = input.ContactEmail;
        agency.ContactPhone = input.ContactPhone;
        agency.Address = input.Address;
        agency.CommissionPercent = input.CommissionPercent ?? 0;
        agency.Status = input.Status;
        agency.CityCode = input.CityCode; // Optional, can be null
        agency.OrgName = input.OrgName;

        agency = await Repository.UpdateAsync(agency);
        return _objectMapper.Map<Agency, AgencyDto>(agency);
    }

    protected override async Task<IQueryable<Agency>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);

        if (input is GetAgencyListDto agencyListInput && !string.IsNullOrWhiteSpace(agencyListInput.Filter))
        {
            query = query.Where(x =>
                x.Status == AgencyStatus.Active &&
                x.Code.Contains(agencyListInput.Filter) ||
                x.Name.Contains(agencyListInput.Filter) ||
                x.ContactEmail.Contains(agencyListInput.Filter) ||
                x.ContactPhone.Contains(agencyListInput.Filter)
            );
        }
        else
        {
            query = query.Where(x => x.Status == AgencyStatus.Active);
        }

        query = query.OrderBy(e => e.Name);
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

    // goi từ Agency > Students Model
    public async Task<PagedResultDto<StudentDto>> GetStudentsListAsync(GetStudentFromAgencyDto input)
    {
        var query = await _studentRepository.GetQueryableAsync();
        query = query.Where(x => x.AgencyId == input.AgencyId);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .OrderByDescending(e => e.CreationTime)
            .ToListAsync();

        return new PagedResultDto<StudentDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Student[], StudentDto[]>(items.ToArray())
        };
    }

    public async Task<PagedResultDto<StudentDto>> GetStudentsAsync(GetStudentListDto dto)
    {
        var query = await _studentRepository.GetQueryableAsync();
        query = query.Where(x => x.AgencyId == dto.AgencyId);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(dto.SkipCount)
            .Take(dto.MaxResultCount)
            .OrderByDescending(e => e.CreationTime)
            .ToListAsync();

        return new PagedResultDto<StudentDto>
        {
            TotalCount = totalCount,
            Items = _objectMapper.Map<Student[], StudentDto[]>(items.ToArray())
        };
    }
} 