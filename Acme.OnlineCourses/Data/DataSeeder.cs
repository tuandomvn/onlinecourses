using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Students;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Identity;
using Acme.OnlineCourses.Permissions;
using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses;

namespace Acme.OnlineCourses.Data;

public class DataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Agency, Guid> _agencyRepository;
    private readonly IRepository<Blog, Guid> _blogRepository;
    private readonly IRepository<Student, Guid> _studentRepository;
    private readonly IRepository<StudentCourse, Guid> _studentCourseRepository;
    private readonly IRepository<StudentAttachment, Guid> _studentAttachmentRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionManager _permissionManager;
    private readonly IRepository<Course, Guid> _courseRepository;
    private readonly IRepository<EmploymentSupport, Guid> _employmentSupportRepository;
    private readonly IRepository<AgentRegister, Guid> _agentRegisterRepository;

    public DataSeeder(
        IRepository<Agency, Guid> agencyRepository,
        IRepository<Blog, Guid> blogRepository,
        IRepository<Student, Guid> studentRepository,
        IRepository<StudentCourse, Guid> studentCourseRepository,
        IRepository<StudentAttachment, Guid> studentAttachmentRepository,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        IUnitOfWorkManager unitOfWorkManager,
        IIdentityRoleRepository roleRepository,
        IdentityRoleManager roleManager,
        IRepository<Course, Guid> courseRepository,
        IPermissionManager permissionManager,
        IRepository<EmploymentSupport, Guid> employmentSupportRepository,
        IRepository<AgentRegister, Guid> agentRegisterRepository)
    {
        _agencyRepository = agencyRepository;
        _blogRepository = blogRepository;
        _studentRepository = studentRepository;
        _studentCourseRepository = studentCourseRepository;
        _studentAttachmentRepository = studentAttachmentRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _unitOfWorkManager = unitOfWorkManager;
        _roleRepository = roleRepository;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _courseRepository = courseRepository;
        _employmentSupportRepository = employmentSupportRepository;
        _agentRegisterRepository = agentRegisterRepository;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        using (var uow = _unitOfWorkManager.Begin())
        {
            try
            {
                await SeedRolesAsync();
                await SeedAgenciesAsync();
                await SeedBlogsAsync();
                await SeedUsersAsync();
                await SeedStudentsAsync();
                await SeedCourseAsync();

                await SeedStudentCoursesAsync();
                await SeedStudentAttachmentsAsync();
                await SeedEmploymentSupportsAsync();
                await SeedAgentRegistersAsync();

                await uow.CompleteAsync();
            }
            catch (Exception)
            {
                await uow.RollbackAsync();
                throw;
            }
        }
    }

    private async Task SeedRolesAsync()
    {
        if (await _roleRepository.GetCountAsync() > 0)
        {
            return;
        }

        var adminRole = new IdentityRole(
            Guid.NewGuid(),
            "admin"
        );

        var studentRole = new IdentityRole(
            Guid.NewGuid(),
            "student"
        );

        var agencyRole = new IdentityRole(
            Guid.NewGuid(),
            "agency"
        );

        await _roleManager.CreateAsync(adminRole);
        await _roleManager.CreateAsync(studentRole);
        await _roleManager.CreateAsync(agencyRole);

        // Grant all permissions to admin role
        var allPermissions = new[]
        {
            // Identity permissions
            "AbpIdentity.Roles",
            "AbpIdentity.Users",
            "AbpIdentity.ClaimTypes",
            "AbpIdentity.SecurityLogs",
            "AbpIdentity.Roles.ManagePermissions",
            "AbpIdentity.Users.ManagePermissions",
            "AbpIdentity.Roles.Create",
            "AbpIdentity.Roles.Edit",
            "AbpIdentity.Roles.Delete",
            "AbpIdentity.Users.Create",
            "AbpIdentity.Users.Edit",
            "AbpIdentity.Users.Delete",

            // Feature management permissions
            "FeatureManagement.ManageHostFeatures",

            // Setting management permissions
            "SettingManagement.ManageHostSettings",

            // Audit logging permissions
            "AuditLogging.Default",

            // Tenant management permissions
            "TenantManagement.Tenants.Default",
            "TenantManagement.Tenants.Create",
            "TenantManagement.Tenants.Delete",

            // Online courses permissions - using constants
            OnlineCoursesPermissions.Students.Default,
            OnlineCoursesPermissions.Students.Create,
            OnlineCoursesPermissions.Students.Edit,
            OnlineCoursesPermissions.Students.Delete,
            
            // Agency permissions - using constants
            OnlineCoursesPermissions.Agencies.Default,
            OnlineCoursesPermissions.Agencies.Create,
            OnlineCoursesPermissions.Agencies.Edit,
            OnlineCoursesPermissions.Agencies.Delete,

            // Blog permissions - using constants
            OnlineCoursesPermissions.Blogs.Default,
            OnlineCoursesPermissions.Blogs.Create,
            OnlineCoursesPermissions.Blogs.Edit,
            OnlineCoursesPermissions.Blogs.Delete,

            // Report permissions - using constants
            OnlineCoursesPermissions.Reports.Default,
            OnlineCoursesPermissions.Reports.Create,
            OnlineCoursesPermissions.Reports.Edit,
            OnlineCoursesPermissions.Reports.Delete,

            // Permission management permissions
            "AbpPermissionManagement.Default",
            "AbpPermissionManagement.ManagePermissions",

            // Identity UI permissions
            "AbpIdentity.Roles",
            "AbpIdentity.Users",
            "AbpIdentity.ClaimTypes",
            "AbpIdentity.SecurityLogs"
        };

        foreach (var permission in allPermissions)
        {
            await _permissionManager.SetForRoleAsync(adminRole.Name, permission, true);
        }

        await _permissionManager.SetForRoleAsync(agencyRole.Name, OnlineCoursesPermissions.Students.Default, true);
    }

    private async Task SeedAgenciesAsync()
    {
        if (await _agencyRepository.GetCountAsync() > 0)
        {
            return;
        }

        var agencies = new[]
        {
            new Agency
            {
                Name = "Hà Nội",
                Code = "AG001",
                Address = "123 Main St, City A",
                Description = "Agency in Hanoi",
                ContactPhone = "0123456789",
                ContactEmail = "test1@gmail.com"
            },
            new Agency
            {
                Name = "Lâm Đồng",
                Code = "AG002",
                Address = "456 Oak St, City B",
                Description = "Agency in Lam Dong",
                ContactPhone = "0123456789",
                ContactEmail = "test1@gmail.com"
            },
            new Agency
            {
                Name = "Thanh Hóa",
                Code = "AG003",
                Address = "789 Pine St, City C",
                Description = "Agency in Thanh Hoa",
                ContactPhone = "0123456789",
                ContactEmail = "test1@gmail.com"
            }
        };

        foreach (var agency in agencies)
        {
            await _agencyRepository.InsertAsync(agency, autoSave: true);
        }
    }

    private async Task SeedBlogsAsync()
    {
        if (await _blogRepository.GetCountAsync() > 0)
        {
            return;
        }

        var blogs = new[]
        {
            new Blog
            {
                Title = "Homepage",
                Content = "Online learning has become increasingly popular in recent years...",
                PublishedDate = DateTime.Now.AddDays(-10),
                Code = "BLG001",
                Language = Language.en
            },
            new Blog
            {
                Title = "Trang chủ",
                Content = "Học trực tuyến đã trở nên ngày càng phổ biến trong những năm gần đây...",
                PublishedDate = DateTime.Now.AddDays(-10),
                Code = "BLG001",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Course-Introduction",
                Content = "Studying online requires different skills and strategies...",
                PublishedDate = DateTime.Now.AddDays(-8),
                Code = "BLG002",
                Language = Language.en
            },
            new Blog
            {
                Title = "Giới thiệu khóa học",
                Content = "Học trực tuyến đòi hỏi những kỹ năng và chiến lược khác nhau...",
                PublishedDate = DateTime.Now.AddDays(-8),
                Code = "BLG002",
                Language = Language.vi
            },
            new Blog
            {
                Title = "TermAndCondition",
                Content = "The education landscape is rapidly evolving...",
                PublishedDate = DateTime.Now.AddDays(-6),
                Code = "BLG003",
                Language = Language.en
            },
            new Blog
            {
                Title = "Điều khoản và điều kiện",
                Content = "Bối cảnh giáo dục đang phát triển nhanh chóng...",
                PublishedDate = DateTime.Now.AddDays(-6),
                Code = "BLG003",
                Language = Language.vi
            },
            new Blog
            {
                Title = "AboutUs",
                Content = "Maintaining motivation in online learning environments can be challenging...",
                PublishedDate = DateTime.Now.AddDays(-4),
                Code = "BLG004",
                Language = Language.en
            },
            new Blog
            {
                Title = "Về chúng tôi",
                Content = "Duy trì động lực trong môi trường học trực tuyến có thể là một thách thức...",
                PublishedDate = DateTime.Now.AddDays(-4),
                Code = "BLG004",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Contact",
                Content = "Online education offers numerous advantages for students...",
                PublishedDate = DateTime.Now.AddDays(-2),
                Code = "BLG005",
                Language = Language.en
            },
            new Blog
            {
                Title = "Liên hệ",
                Content = "Giáo dục trực tuyến mang lại nhiều lợi ích cho học viên...",
                PublishedDate = DateTime.Now.AddDays(-2),
                Code = "BLG005",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Building a Successful Online Learning Community",
                Content = "Creating an engaging online learning community is essential for student success...",
                PublishedDate = DateTime.Now.AddDays(-1),
                Code = "BLG006",
                Language = Language.en
            },
            new Blog
            {
                Title = "Xây dựng cộng đồng học trực tuyến thành công",
                Content = "Tạo một cộng đồng học trực tuyến hấp dẫn là điều cần thiết cho sự thành công của học viên...",
                PublishedDate = DateTime.Now.AddDays(-1),
                Code = "BLG006",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Technology Trends in Education",
                Content = "Emerging technologies are reshaping the way we learn and teach...",
                PublishedDate = DateTime.Now,
                Code = "BLG007",
                Language = Language.en
            },
            new Blog
            {
                Title = "Xu hướng công nghệ trong giáo dục",
                Content = "Các công nghệ mới đang định hình lại cách chúng ta học và dạy...",
                PublishedDate = DateTime.Now,
                Code = "BLG007",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Balancing Work and Online Learning",
                Content = "Many students struggle to balance their professional and academic lives...",
                PublishedDate = DateTime.Now.AddDays(1),
                Code = "BLG008",
                Language = Language.en
            },
            new Blog
            {
                Title = "Cân bằng giữa công việc và học trực tuyến",
                Content = "Nhiều học viên gặp khó khăn trong việc cân bằng giữa công việc và học tập...",
                PublishedDate = DateTime.Now.AddDays(1),
                Code = "BLG008",
                Language = Language.vi
            },
            new Blog
            {
                Title = "The Role of AI in Education",
                Content = "Artificial Intelligence is transforming educational experiences...",
                PublishedDate = DateTime.Now.AddDays(2),
                Code = "BLG009",
                Language = Language.en
            },
            new Blog
            {
                Title = "Vai trò của AI trong giáo dục",
                Content = "Trí tuệ nhân tạo đang thay đổi trải nghiệm giáo dục...",
                PublishedDate = DateTime.Now.AddDays(2),
                Code = "BLG009",
                Language = Language.vi
            },
            new Blog
            {
                Title = "Creating Engaging Online Content",
                Content = "Learn how to create content that keeps students engaged and motivated...",
                PublishedDate = DateTime.Now.AddDays(3),
                Code = "BLG010",
                Language = Language.en
            },
            new Blog
            {
                Title = "Tạo nội dung trực tuyến hấp dẫn",
                Content = "Học cách tạo nội dung giúp học viên luôn hứng thú và có động lực...",
                PublishedDate = DateTime.Now.AddDays(3),
                Code = "BLG010",
                Language = Language.vi
            }
        };

        foreach (var blog in blogs)
        {
            blog.Summary = "TBD"; // Default summary
            blog.IsPublished = true;
            await _blogRepository.InsertAsync(blog, autoSave: true);
        }
    }

    private async Task SeedUsersAsync()
    {
        if (await _userRepository.GetCountAsync() > 0)
        {
            return;
        }

        var adminUser = new IdentityUser(
            Guid.NewGuid(),
            "admin1",
            "admin@onlinecourses.com"
        );

        await _userManager.CreateAsync(adminUser, "1q2w3E*");
        await _userManager.AddToRoleAsync(adminUser, "admin");

        var studentUser = new IdentityUser(
            Guid.NewGuid(),
            "student1",
            "student@onlinecourses.com"
        );

        await _userManager.CreateAsync(studentUser, "1q2w3E*");
        await _userManager.AddToRoleAsync(studentUser, "student");

        // Get first agency for the agency user
        var firstAgency = await _agencyRepository.FirstOrDefaultAsync();
        if (firstAgency != null)
        {
            var agencyUser = new IdentityUser(
                Guid.NewGuid(),
                "agency1",
                "agency@onlinecourses.com"
            );

            // Add agencyId as extra property
            agencyUser.SetProperty("AgencyId", firstAgency.Id.ToString());

            await _userManager.CreateAsync(agencyUser, "1q2w3E*");
            await _userManager.AddToRoleAsync(agencyUser, "agency");
        }
    }

    private async Task SeedStudentsAsync()
    {
        if (await _studentRepository.GetCountAsync() > 0)
        {
            return;
        }

        var students = new[]
        {
            new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St, City",
                TestStatus = TestStatus.NotTaken,
                PaymentStatus = PaymentStatus.Paid,
                AccountStatus = AccountStatus.NotSent,
                AgreeToTerms = true
            },
            new Student
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "0987654321",
                DateOfBirth = new DateTime(1992, 5, 15),
                Address = "456 Oak St, Town",
                TestStatus = TestStatus.NotTaken,
                PaymentStatus = PaymentStatus.Paid,
                AccountStatus = AccountStatus.NotSent,
                AgreeToTerms = true
            },
            new Student
            {
                FirstName = "Mike",
                LastName = "Johnson",
                Email = "mike.johnson@example.com",
                PhoneNumber = "5551234567",
                DateOfBirth = new DateTime(1988, 12, 25),
                Address = "789 Pine St, Village",
                TestStatus = TestStatus.NotTaken,
                PaymentStatus = PaymentStatus.Paid,
                AccountStatus = AccountStatus.NotSent,
                AgreeToTerms = true
            }
        };

        foreach (var student in students)
        {
            await _studentRepository.InsertAsync(student, autoSave: true);
        }
    }

    public async Task SeedCourseAsync()
    {
        if (await _courseRepository.GetCountAsync() > 0)
        {
            return;
        }

        await _courseRepository.InsertAsync(
            new Course
            {
                Code = "TESOL-101",
                Name = "TESOL Foundation",
                Description = "Basic TESOL course for beginners",
                Price = 999.99m,
                Duration = 120,
                Status = CourseStatus.Active
            },
            autoSave: true
        );

        await _courseRepository.InsertAsync(
            new Course
            {
                Code = "TESOL-201",
                Name = "TESOL Advanced",
                Description = "Advanced TESOL course for experienced teachers",
                Price = 1499.99m,
                Duration = 180,
                Status = CourseStatus.Active
            },
            autoSave: true
        );
    }
    private async Task SeedStudentCoursesAsync()
    {
        if (await _studentCourseRepository.GetCountAsync() > 0)
        {
            return;
        }

        var students = await _studentRepository.GetListAsync();
        var coursesData = await _courseRepository.GetListAsync();
        var courses = new[]
        {
            new StudentCourse
            {
                StudentId = students[0].Id,
                CourseId = coursesData[0].Id,
                RegistrationDate = DateTime.Now.AddDays(-30),
                CourseNote = "Basic programming concepts and languages."
            },
            new StudentCourse
            {
                StudentId = students[0].Id,
                CourseId = coursesData[0].Id,
                RegistrationDate = DateTime.Now.AddDays(-20),
                CourseNote= "Learn the fundamentals of web development."
            },
            new StudentCourse
            {
                StudentId = students[1].Id,
                CourseId = coursesData[0].Id,
                RegistrationDate = DateTime.Now.AddDays(-15),
                CourseNote = "Deep dive into JavaScript programming."
            },
            new StudentCourse
            {
                StudentId = students[2].Id,
                CourseId = coursesData[0].Id,
                RegistrationDate = DateTime.Now.AddDays(-10),
                CourseNote = "Learn how to design and manage databases."
            }
        };

        foreach (var course in courses)
        {
            await _studentCourseRepository.InsertAsync(course, autoSave: true);
        }
    }

    private async Task SeedStudentAttachmentsAsync()
    {
        if (await _studentAttachmentRepository.GetCountAsync() > 0)
        {
            return;
        }

        var students = await _studentRepository.GetListAsync();
        var attachments = new[]
        {
            new StudentAttachment
            {
                StudentId = students[0].Id,
                FileName = "ID_Card_John.pdf",
                FilePath = "/uploads/students/id_cards/ID_Card_John.pdf",
                UploadDate = DateTime.Now.AddDays(-25)
            },
            new StudentAttachment
            {
                StudentId = students[0].Id,
                FileName = "Certificate_John.pdf",
                FilePath = "/uploads/students/certificates/Certificate_John.pdf",
                UploadDate = DateTime.Now.AddDays(-15)
            },
            new StudentAttachment
            {
                StudentId = students[1].Id,
                FileName = "ID_Card_Jane.pdf",
                FilePath = "/uploads/students/id_cards/ID_Card_Jane.pdf",
                UploadDate = DateTime.Now.AddDays(-20)
            },
            new StudentAttachment
            {
                StudentId = students[2].Id,
                FileName = "ID_Card_Mike.pdf",
                FilePath = "/uploads/students/id_cards/ID_Card_Mike.pdf",
                UploadDate = DateTime.Now.AddDays(-12)
            }
        };

        foreach (var attachment in attachments)
        {
            attachment.Description = "Uploaded by data seeder";
            await _studentAttachmentRepository.InsertAsync(attachment, autoSave: true);
        }
    }

    private async Task SeedEmploymentSupportsAsync()
    {
        if (await _employmentSupportRepository.GetCountAsync() > 0)
        {
            return;
        }
        var supports = new[]
        {
            new EmploymentSupport
            {
                FullName = "Nguyen Van A",
                DateOfBirth = new DateTime(1995, 5, 20),
                PhoneNumber = "0912345678",
                Email = "vana@example.com",
                Address = "123 Đường A, Quận B, TP. C",
                CourseCompletionDate = new DateTime(2024, 6, 1),
                Message = "Tôi cần hỗ trợ việc làm sau khi hoàn thành khóa học."
            },
            new EmploymentSupport
            {
                FullName = "Tran Thi B",
                DateOfBirth = new DateTime(1998, 8, 15),
                PhoneNumber = "0987654321",
                Email = "thib@example.com",
                Address = "456 Đường X, Quận Y, TP. Z",
                CourseCompletionDate = new DateTime(2024, 6, 5),
                Message = "Xin tư vấn về các cơ hội việc làm phù hợp."
            }
        };
        foreach (var support in supports)
        {
            await _employmentSupportRepository.InsertAsync(support, autoSave: true);
        }
    }

    private async Task SeedAgentRegistersAsync()
    {
        if (await _agentRegisterRepository.GetCountAsync() > 0)
        {
            return;
        }
        var agents = new[]
        {
            new AgentRegister
            {
                FullName = "Nguyen Van C",
                PhoneNumber = "0901234567",
                Email = "c.agent@example.com",
                Address = "789 Đường Z, Quận W, TP. Q",
                OrganizationName = "Tổ chức ABC",
                Position = "Trưởng phòng",
                Message = "Tôi muốn đăng ký làm đại lý."
            },
            new AgentRegister
            {
                FullName = "Le Thi D",
                PhoneNumber = "0934567890",
                Email = "d.agent@example.com",
                Address = "321 Đường M, Quận N, TP. P",
                OrganizationName = "Tổ chức XYZ",
                Position = "Nhân viên",
                Message = "Xin tư vấn về chương trình đại lý."
            }
        };
        foreach (var agent in agents)
        {
            await _agentRegisterRepository.InsertAsync(agent, autoSave: true);
        }
    }
}