using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses.Extensions;
using Acme.OnlineCourses.Helpers;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Students.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using static Acme.OnlineCourses.OnlineCoursesConsts;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

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
    private readonly IMailService _mailService;
    public StudentAppService(
        IRepository<Student, Guid> repository,
        ICurrentUser currentUser,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        ILogger<StudentAppService> logger,
        IRepository<StudentAttachment, Guid> attachmentRepository,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Course, Guid> courseRepository,
        IMailService mailService)
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
        _mailService = mailService;


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
                x.Fullname.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.PhoneNumber.Contains(input.Filter)
            );
        }

        return query;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("api/app/student/register-student")]
    public async Task<StudentDto> RegisterStudentAsync([FromForm] RegisterStudentDto input, [FromForm] List<IFormFile> files)
    {
        var student = new Student
        {
            Fullname = input.Fullname,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            DateOfBirth = input.DateOfBirth,
            Address = input.Address,
            AgencyId = input.AgencyId,
            AgreeToTerms = input.AgreeToTerms
        };
        await _studentRepository.InsertAsync(student, autoSave: true);

        var firstCourse = await _courseRepository.FirstOrDefaultAsync();

        var studentAttachments = new List<StudentAttachment>();
        if (files != null && files.Any())
        {
            var allowedExtensions = new[] { ".txt", ".csv", ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".xls", ".xlsx" };
            const int maxFileSize = 10 * 1024 * 1024; // 10MB

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //Validate file extension
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        _logger.LogWarning($"Invalid file extension: {extension}");
                        throw new UserFriendlyException($"File type not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
                    }

                    // Validate file size
                    if (file.Length > maxFileSize)
                    {
                        _logger.LogWarning($"File too large: {file.Length} bytes");
                        throw new UserFriendlyException($"File is too large. Maximum size allowed is 10MB.");
                    }

                    // Create upload directory if not exists
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "students", student.Id.ToString(), firstCourse.Id.ToString().Substring(0, 4));
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

                    studentAttachments.Add(studentAttachment);
                }
            }
        }


        // Get first course and create StudentCourse record
        await InsertStudentCourse(student, input, firstCourse);

        await _attachmentRepository.InsertManyAsync(studentAttachments, autoSave: true);

        var isUserExists = await IsUserExistsAsync(input.Email);
        if (!isUserExists)
        {
            _logger.LogInformation($"User with email {input.Email} does not exist.");
            var studentUser = new IdentityUser(Guid.NewGuid(), input.Email, input.Email);

            var password = PasswordGenerator.GenerateSecurePassword(8);

            await _userManager.CreateAsync(studentUser, password);
            await _userManager.AddToRoleAsync(studentUser, Roles.Student);

            // Send welcome email
            _mailService.SendWelcomeEmailAsync(new WelcomeRequest
            {
                ToEmail = input.Email,
                UserName = input.Email,
                Password = password,
                CourseName = firstCourse.Name,
            });

          
            var adminUsers = await _userManager.GetUsersInRoleAsync(Roles.Administrator);
            foreach (var admin in adminUsers)
            {
                _mailService.SendNotifyToAdminsAsync(new NotityToAdminRequest
                {
                    ToEmail = admin.Email,
                    StudentName = student.Fullname,
                    StudentEmail = student.Email,
                    CourseName = firstCourse.Name
                });
            }
        }
        else
        {
            _logger.LogWarning($"User with email {input.Email} already exists.");
            throw new UserFriendlyException("A user with this email already exists. Please use a different email address.");
        }

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

    private async Task InsertStudentCourse(Student student, RegisterStudentDto input, Course firstCourse)
    {
        if (firstCourse != null)
        {
            var studentCourse = new StudentCourse
            {
                StudentId = student.Id,
                CourseId = firstCourse.Id,
                RegistrationDate = DateTime.Now,
                ExpectedStudyDate = input.ExpectedStudyDate.HasValue ? input.ExpectedStudyDate.Value : DateTime.MinValue,
                CourseStatus = StudentCourseStatus.Inprogress,
                StudentNote = input.StudentNote ?? "TBD"
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
        // Kiểm tra role - Agency không được phép upload
        if (_currentUser.IsAuthenticated && _currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
        {
            throw new UserFriendlyException("Agency users are not allowed to upload attachments.");
        }

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

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    public async Task DeleteAttachmentAsync(Guid attachmentId)
    {
        // Kiểm tra role - Agency không được phép delete
        if (_currentUser.IsAuthenticated && _currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
        {
            throw new UserFriendlyException("Agency users are not allowed to delete attachments.");
        }

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

    [Authorize]
    [HttpPost]
    [Route("api/app/student/update")]
    public async Task<StudentDto> UpdateStudentAsync([FromForm] UpdateStudentDto input, [FromForm] List<IFormFile> files)
    {
        // Kiểm tra role - Agency không được phép update
        if (_currentUser.IsAuthenticated && _currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
        {
            throw new UserFriendlyException("Agency users are not allowed to update student information.");
        }

        var student = await _studentRepository.GetAsync(input.Id);

        // Update only allowed fields
        student.Fullname = input.FullName;
        student.PhoneNumber = input.PhoneNumber;
        student.DateOfBirth = input.DateOfBirth;
        student.Address = input.Address;

        await _studentRepository.UpdateAsync(student);

        bool hasNewAttachment = false;

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
                    hasNewAttachment = true;
                }
            }
        }

        // Notify all admins if there was a new attachment
        if (hasNewAttachment)
        {
            var adminUsers = await _userManager.GetUsersInRoleAsync(Roles.Administrator);
            foreach (var admin in adminUsers)
            {
                _mailService.SendNotifyUpdateAttachmentAsync(new NotifyUpdateAttachmentRequest
                {
                    ToEmail = admin.Email,
                    StudentEmail = student.Email,
                });
            }
        }

        return ObjectMapper.Map<Student, StudentDto>(student);
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    public async Task<PagedResultDto<AdminViewStudentDto>> GetStudentsWithCoursesAsync(GetStudentListDto input)
    {
        var query = await _studentRepository.GetQueryableAsync();

        // Include StudentCourse và Course để lấy thông tin khóa học
        query = query.Include(x => x.Courses).ThenInclude(c => c.Course);

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.Fullname.Contains(input.Filter) ||
                x.Email.Contains(input.Filter) ||
                x.PhoneNumber.Contains(input.Filter)
            );
        }

        //filter by user logged in and agency if applicable
        if (_currentUser.IsAuthenticated && !string.IsNullOrEmpty(_currentUser.Email))
        {
            if (_currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
            {
                // Lấy agencyId
                if (_currentUser != null && _userRepository != null)
                {
                    var agencyId = await _currentUser.GetAgencyIdAsync(_userRepository);
                    if (agencyId.HasValue)
                    {
                        query = query.Where(x => x.AgencyId == agencyId.Value);
                    }
                }
            }
        }

        if (input.AgencyId.HasValue)
        {
            query = query.Where(x => x.AgencyId == input.AgencyId.Value);
        }

        if (input.CourseStatus.HasValue)
        {
            query = query.Where(x => x.Courses.Any(c => c.CourseStatus == input.CourseStatus.Value));
        }

        var totalCount = await query.CountAsync();
        var students = await query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync();

        var dtos = new List<AdminViewStudentDto>();
        foreach (var student in students)
        {
            if (student.Courses.Count > 0)
            {
                foreach (var course in student.Courses)
                {
                    dtos.Add(new AdminViewStudentDto
                    {
                        Id = student.Id,
                        FullName = student.Fullname,
                        Email = student.Email,
                        PhoneNumber = student.PhoneNumber,
                        Address = student.Address,
                        DateOfBirth = student.DateOfBirth,
                        AgreeToTerms = student.AgreeToTerms,
                        RegistrationDate = course.RegistrationDate,
                        CourseId = course.CourseId,
                        CourseName = course.Course?.Name,
                        CourseStatus = course.CourseStatus,
                        TestStatus = course.TestStatus,
                        PaymentStatus = course.PaymentStatus,
                        CourseNote = course.StudentNote,
                        AgencyId = student.AgencyId,
                        CreationTime = student.CreationTime
                    });
                }
            }
            else
            {
                // Nếu student không có course nào, vẫn hiển thị với thông tin cơ bản
                dtos.Add(new AdminViewStudentDto
                {
                    Id = student.Id,
                    FullName = student.Fullname,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber,
                    Address = student.Address,
                    DateOfBirth = student.DateOfBirth,
                    AgreeToTerms = student.AgreeToTerms,
                    AgencyId = student.AgencyId,
                    CreationTime = student.CreationTime
                });
            }
        }

        _logger.LogInformation($"GetStudentsWithCoursesAsync - TotalCount: {totalCount}, ItemsCount: {dtos.Count}");

        return new PagedResultDto<AdminViewStudentDto>
        {
            TotalCount = totalCount,
            Items = dtos
        };
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    public async Task<UpdateStudentCourseDto> GetStudentCourseAsync(Guid studentId, Guid courseId)
    {
        var query = await _studentRepository.GetQueryableAsync();
        var student = await query
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(x => x.Id == studentId);

        if (student == null)
        {
            throw new UserFriendlyException("Student not found");
        }

        var studentCourse = student.Courses.FirstOrDefault(x => x.CourseId == courseId);
        if (studentCourse == null)
        {
            throw new UserFriendlyException("Student course not found");
        }

        return new UpdateStudentCourseDto
        {
            StudentId = studentId,
            CourseId = courseId,
            CourseStatus = studentCourse.CourseStatus,
            TestStatus = studentCourse.TestStatus,
            PaymentStatus = studentCourse.PaymentStatus,
            StudentNote = studentCourse.StudentNote,
            AdminNote = studentCourse.AdminNote,
            Email = student.Email
        };
    }

    [Authorize(OnlineCoursesPermissions.Students.Default)]
    public async Task UpdateStudentCourseAsync(UpdateStudentCourseDto input)
    {
        // Kiểm tra role - Agency không được phép update
        if (_currentUser.IsAuthenticated && _currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency))
        {
            throw new UserFriendlyException("Agency users are not allowed to update student course information.");
        }

        var query = await _studentRepository.GetQueryableAsync();
        var student = await query
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(x => x.Id == input.StudentId);

        if (student == null)
        {
            throw new UserFriendlyException("Student not found");
        }

        var studentCourse = student.Courses.FirstOrDefault(x => x.CourseId == input.CourseId);
        if (studentCourse == null)
        {
            throw new UserFriendlyException("Student course not found");
        }

        // Cập nhật chỉ thông tin trong StudentCourse
        studentCourse.CourseStatus = input.CourseStatus;
        studentCourse.TestStatus = input.TestStatus;
        studentCourse.PaymentStatus = input.PaymentStatus;
        studentCourse.StudentNote = input.StudentNote;
        studentCourse.AdminNote = input.AdminNote;

        await _studentRepository.UpdateAsync(student);
    }
}