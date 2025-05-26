using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.OnlineCourses.Entities;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Students;

public class StudentAppService : 
    CrudAppService<
        Student,
        StudentDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateStudentDto,
        CreateUpdateStudentDto>,
    IStudentAppService
{
    public StudentAppService(IRepository<Student, Guid> repository)
        : base(repository)
    {
    }

    protected override async Task<IQueryable<Student>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);
        
        // Add default sorting by registration date if no sorting is specified
        if (string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = query.OrderByDescending(x => x.RegistrationDate);
        }
        
        return query;
    }
} 