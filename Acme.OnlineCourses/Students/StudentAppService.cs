using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.OnlineCourses.Students;

public class StudentAppService : CrudAppService<
    Student,
    StudentDto,
    Guid,
    GetStudentListDto,
    CreateUpdateStudentDto>,
    IStudentAppService
{
    private readonly IRepository<Student, Guid> _studentRepository;

    public StudentAppService(IRepository<Student, Guid> repository)
        : base(repository)
    {
        _studentRepository = repository;
        GetPolicyName = OnlineCoursesPermissions.Students.Default;
        GetListPolicyName = OnlineCoursesPermissions.Students.Default;
        CreatePolicyName = OnlineCoursesPermissions.Students.Create;
        UpdatePolicyName = OnlineCoursesPermissions.Students.Edit;
        DeletePolicyName = OnlineCoursesPermissions.Students.Delete;
    }

    protected override async Task<IQueryable<Student>> CreateFilteredQueryAsync(GetStudentListDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.FirstName.Contains(input.Filter) ||
                x.LastName.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.PhoneNumber.Contains(input.Filter) ||
                x.IdentityNumber.Contains(input.Filter)
            );
        }

        if (input.AgencyId.HasValue)
        {
            query = query.Where(x => x.AgencyId == input.AgencyId.Value);
        }

        if (input.TestStatus.HasValue)
        {
            query = query.Where(x => x.TestStatus == input.TestStatus.Value);
        }

        if (input.PaymentStatus.HasValue)
        {
            query = query.Where(x => x.PaymentStatus == input.PaymentStatus.Value);
        }

        if (input.AccountStatus.HasValue)
        {
            query = query.Where(x => x.AccountStatus == input.AccountStatus.Value);
        }

        if (input.CourseStatus.HasValue)
        {
            query = query.Where(x => x.CourseStatus == input.CourseStatus.Value);
        }

        return query;
    }

    public async Task<StudentDto> RegisterStudentAsync(RegisterStudentDto input)
    {
        var student = new Student
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            DateOfBirth = input.DateOfBirth,
            IdentityNumber = input.IdentityNumber,
            Address = input.Address,
            AgencyId = input.AgencyId,
            AgreeToTerms = input.AgreeToTerms,
            //TestStatus = TestStatus.NotStarted,
            //PaymentStatus = PaymentStatus.Unpaid,
            //AccountStatus = AccountStatus.Pending,
            //CourseStatus = CourseStatus.NotStarted
        };

        await _studentRepository.InsertAsync(student, autoSave: true);

        return ObjectMapper.Map<Student, StudentDto>(student);
    }
} 