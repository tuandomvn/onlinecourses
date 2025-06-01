using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using System.Linq;
using Volo.Abp.Identity;
using Acme.OnlineCourses.Extensions;

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
    private readonly ICurrentUser _currentUser;
    private readonly IIdentityUserRepository _userRepository;

    public StudentAppService(IRepository<Student, Guid> repository, ICurrentUser currentUser, IIdentityUserRepository userRepository)
        : base(repository)
    {
        _studentRepository = repository;
        _currentUser = currentUser;
        _userRepository = userRepository;
        //GetPolicyName = OnlineCoursesPermissions.Students.Default;
        //GetListPolicyName = OnlineCoursesPermissions.Students.Default;
        //CreatePolicyName = OnlineCoursesPermissions.Students.Create;
        //UpdatePolicyName = OnlineCoursesPermissions.Students.Edit;
        //DeletePolicyName = OnlineCoursesPermissions.Students.Delete;
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

        //filter by user logged in and agency if applicable
        if (_currentUser.IsAuthenticated && !string.IsNullOrEmpty(_currentUser.Email) 
            && _currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
        {
            //var user = await _currentUser.GetUserAsync(_userRepository);

            // Lấy agencyId
            var agencyId = await _currentUser.GetAgencyIdAsync(_userRepository);

            query = query.Where(x => x.AgencyId == agencyId);
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