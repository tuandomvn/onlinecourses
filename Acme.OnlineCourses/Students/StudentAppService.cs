using Acme.OnlineCourses.Extensions;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using static Acme.OnlineCourses.OnlineCoursesConsts;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Acme.OnlineCourses.Courses;

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
    private readonly ILogger<StudentAppService> _logger;
    private readonly IdentityUserManager _userManager;
    private readonly IRepository<StudentAttachment, Guid> _attachmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Course, Guid> _courseRepository;

    public StudentAppService(
        IRepository<Student, Guid> repository,
        ICurrentUser currentUser,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        ILogger<StudentAppService> logger,
        IRepository<StudentAttachment, Guid> attachmentRepository,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Course, Guid> courseRepository)
        : base(repository)
    {
        _studentRepository = repository;
        _currentUser = currentUser;
        _userRepository = userRepository;
        _logger = logger;
        _userManager = userManager;
        _attachmentRepository = attachmentRepository;
        _httpContextAccessor = httpContextAccessor;
        _courseRepository = courseRepository;

        GetPolicyName = OnlineCoursesPermissions.Students.Default;
        GetListPolicyName = OnlineCoursesPermissions.Students.Default;
        CreatePolicyName = OnlineCoursesPermissions.Students.Create;
        UpdatePolicyName = OnlineCoursesPermissions.Students.Edit;
        DeletePolicyName = OnlineCoursesPermissions.Students.Delete;
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    protected override async Task<IQueryable<Student>> CreateFilteredQueryAsync(GetStudentListDto input)
    {
        var query = await base.CreateFilteredQueryAsync(input);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.FirstName.Contains(input.Filter) ||
                x.LastName.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.PhoneNumber.Contains(input.Filter)
            );
        }

        //filter by user logged in and agency if applicable
        if (_currentUser.IsAuthenticated && !string.IsNullOrEmpty(_currentUser.Email))
        {
            if(_currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
            {
                // Lấy agencyId
                if (_currentUser != null && _userRepository != null)
                {
                    var agencyId = _currentUser.GetAgencyIdAsync(_userRepository);
                    if (agencyId != null)
                    {
                        query = query.Where(x => x.AgencyId == agencyId);
                    }
                }
            }
        }
        //else
        //{
        //    //No result
        //    query = query.Where(x => x.AgencyId == Guid.NewGuid());
        //}

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
            query = query.Where(x => x.Courses.Any(c => c.CourseStatus == input.CourseStatus.Value));
        }

        return query;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("api/app/student/register-student")]
    public async Task<StudentDto> RegisterStudentAsync([FromForm] RegisterStudentDto input, [FromForm] List<IFormFile> files)
    {
        var isUserExists = await IsUserExistsAsync(input.Email);
        if (!isUserExists)
        {
            _logger.LogWarning($"User with email {input.Email} does not exist.");
            var studentUser = new IdentityUser(Guid.NewGuid(), input.Email, input.Email);

            await _userManager.CreateAsync(studentUser, "1q2w3E*");//TODO
            await _userManager.AddToRoleAsync(studentUser, Roles.Student);
            //TODO: send mail
        }

        var student = new Student
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            DateOfBirth = input.DateOfBirth,
            Address = input.Address,
            AgencyId = input.AgencyId,
            AgreeToTerms = input.AgreeToTerms,
            StudentNote = input.StudentNote,
        };

        await _studentRepository.InsertAsync(student, autoSave: true);

        // Get first course and create StudentCourse record
        await InsertStudentCourse(student);

        // Handle file uploads if any
        if (files != null && files.Any())
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Create upload directory if not exists
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "students", student.Id.ToString());
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Generate unique filename
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadDir, fileName);

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Create attachment record
                    var studentAttachment = new StudentAttachment
                    {
                        StudentId = student.Id,
                        FileName = file.FileName,
                        FilePath = $"/uploads/students/{student.Id}/{fileName}",
                        Description = "Uploaded during registration"
                    };

                    await _attachmentRepository.InsertAsync(studentAttachment, autoSave: true);
                }
            }
        }

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

    private async Task InsertStudentCourse(Student student)
    {
        var firstCourse = await _courseRepository.FirstOrDefaultAsync();
        if (firstCourse != null)
        {
            var studentCourse = new StudentCourse
            {
                StudentId = student.Id,
                CourseId = firstCourse.Id,
                RegistrationDate = DateTime.Now,
                CourseStatus = StudentCourseStatus.Inprogress,
                CourseNote = "TBD"
            };
            student.Courses.Add(studentCourse);
            await _studentRepository.UpdateAsync(student);
        }
    }

    [Authorize]
    public async Task<StudentDto> GetByEmailAsync(string email)
    {
        var query = await _studentRepository.GetQueryableAsync();
        var student = await query
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (student == null)
        {
            throw new UserFriendlyException("Student not found");
        }

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

    [Authorize]
    public async Task<ProfileStudentDto> GetProfileStudentByEmailAsync(string email)
    {
        var query = await _studentRepository.GetQueryableAsync();
        var student = await query
            .Include(x => x.Attachments)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (student == null)
        {
            return null;
        }

        return ObjectMapper.Map<Student, ProfileStudentDto>(student);
    }

    public async Task<bool> IsUserExistsAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        var normalizedEmail = email.ToUpperInvariant();
        var user = await _userRepository.FindByNormalizedEmailAsync(normalizedEmail);
        return user != null;
    }

    public async Task<List<StudentAttachmentDto>> GetAttachmentsAsync(Guid studentId)
    {
        var attachments = await _attachmentRepository.GetListAsync(x => x.StudentId == studentId);
        return ObjectMapper.Map<List<StudentAttachment>, List<StudentAttachmentDto>>(attachments);
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    [HttpPost]
    [Route("api/app/student/upload")]
    public async Task<StudentAttachmentDto> UploadAttachmentAsync([FromForm] Guid studentId, [FromForm] IFormFile file, [FromForm] string description)
    {
        if (file == null || file.Length == 0)
        {
            throw new UserFriendlyException("File is empty");
        }

        // Create upload directory if not exists
        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "students", studentId.ToString());
        if (!Directory.Exists(uploadDir))
        {
            Directory.CreateDirectory(uploadDir);
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadDir, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Create attachment record
        var attachment = new StudentAttachment
        {
            StudentId = studentId,
            FileName = file.FileName,
            FilePath = $"/uploads/students/{studentId}/{fileName}",
            Description = description
        };

        await _attachmentRepository.InsertAsync(attachment, autoSave: true);

        return ObjectMapper.Map<StudentAttachment, StudentAttachmentDto>(attachment);
    }

    public async Task DeleteAttachmentAsync(Guid attachmentId)
    {
        var attachment = await _attachmentRepository.GetAsync(attachmentId);

        // Delete file
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        // Delete record
        await _attachmentRepository.DeleteAsync(attachment);
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    public override async Task<StudentDto> UpdateAsync(Guid id, CreateUpdateStudentDto input)
    {
        var student = await _studentRepository.GetAsync(id);

        // Update only allowed fields
        student.FirstName = input.FirstName;
        student.LastName = input.LastName;
        student.PhoneNumber = input.PhoneNumber;
        student.DateOfBirth = input.DateOfBirth;
        student.Address = input.Address;

        // Handle attachments
        //if (input.Attachments != null && input.Attachments.Any())
        //{
        //    // Delete existing attachments that are not in the new list
        //    var existingAttachments = await _attachmentRepository.GetListAsync(x => x.StudentId == id);
        //    var attachmentsToDelete = existingAttachments.Where(x => !input.Attachments.Any(a => a.FilePath == x.FilePath));
        //    foreach (var attachment in attachmentsToDelete)
        //    {
        //        await _attachmentRepository.DeleteAsync(attachment);
        //    }

        //    // Add new attachments
        //    foreach (var attachmentDto in input.Attachments)
        //    {
        //        if (!existingAttachments.Any(x => x.FilePath == attachmentDto.FilePath))
        //        {
        //            var attachment = new StudentAttachment
        //            {
        //                StudentId = id,
        //                FileName = attachmentDto.FileName,
        //                FilePath = attachmentDto.FilePath,
        //                Description = attachmentDto.Description
        //            };
        //            await _attachmentRepository.InsertAsync(attachment);
        //        }
        //    }
        //}

        await _studentRepository.UpdateAsync(student);

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

    //[Authorize]
    [HttpPost]
    [Route("api/app/student/update")]
    public async Task<StudentDto> UpdateStudentAsync([FromForm] UpdateStudentDto input, [FromForm] List<IFormFile> files)
    {
        var student = await _studentRepository.GetAsync(input.Id);

        // Update only allowed fields
        student.FirstName = input.FirstName;
        student.LastName = input.LastName;
        student.PhoneNumber = input.PhoneNumber;
        student.DateOfBirth = input.DateOfBirth;
        student.Address = input.Address;

        await _studentRepository.UpdateAsync(student);

        // Handle file uploads if any
        if (files != null && files.Any())
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Create upload directory if not exists
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "students", student.Id.ToString());
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    // Generate unique filename
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadDir, fileName);

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Create attachment record
                    var studentAttachment = new StudentAttachment
                    {
                        StudentId = student.Id,
                        FileName = file.FileName,
                        FilePath = $"/uploads/students/{student.Id}/{fileName}",
                        Description = "Uploaded during registration"
                    };

                    await _attachmentRepository.InsertAsync(studentAttachment, autoSave: true);
                }
            }
        }

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

}