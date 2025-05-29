using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Identity;
using Acme.OnlineCourses.Permissions;

namespace Acme.OnlineCourses.Data;

public class DataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Agency, Guid> _agencyRepository;
    private readonly IRepository<Blog, Guid> _blogRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IIdentityRoleRepository _roleRepository;
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionManager _permissionManager;

    public DataSeeder(
        IRepository<Agency, Guid> agencyRepository,
        IRepository<Blog, Guid> blogRepository,
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        IUnitOfWorkManager unitOfWorkManager,
        IIdentityRoleRepository roleRepository,
        IdentityRoleManager roleManager,
        IPermissionManager permissionManager)
    {
        _agencyRepository = agencyRepository;
        _blogRepository = blogRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _unitOfWorkManager = unitOfWorkManager;
        _roleRepository = roleRepository;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
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

            // Online courses permissions
            "OnlineCourses.Students.Default",
            "OnlineCourses.Students.Create",
            "OnlineCourses.Students.Edit",
            "OnlineCourses.Students.Delete",
            "OnlineCourses.Agencies.Default",
            "OnlineCourses.Agencies.Create",
            "OnlineCourses.Agencies.Edit",
            "OnlineCourses.Agencies.Delete",

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
                Title = "Getting Started with Online Learning",
                Content = "Online learning has become increasingly popular in recent years...",
                PublishedDate = DateTime.Now.AddDays(-10),
                Code = "BLG001"
            },
            new Blog
            {
                Title = "Tips for Effective Online Study",
                Content = "Studying online requires different skills and strategies...",
                PublishedDate = DateTime.Now.AddDays(-8),
                Code = "BLG002"
            },
            new Blog
            {
                Title = "The Future of Education",
                Content = "The education landscape is rapidly evolving...",
                PublishedDate = DateTime.Now.AddDays(-6),
                Code = "BLG003"
            },
            new Blog
            {
                Title = "How to Stay Motivated While Learning Online",
                Content = "Maintaining motivation in online learning environments can be challenging...",
                PublishedDate = DateTime.Now.AddDays(-4),
                Code = "BLG004"
            },
            new Blog
            {
                Title = "The Benefits of Online Education",
                Content = "Online education offers numerous advantages for students...",
                PublishedDate = DateTime.Now.AddDays(-2),
                Code = "BLG005"
            },
            new Blog
            {
                Title = "Building a Successful Online Learning Community",
                Content = "Creating an engaging online learning community is essential for student success...",
                PublishedDate = DateTime.Now.AddDays(-1),
                Code = "BLG006"
            },
            new Blog
            {
                Title = "Technology Trends in Education",
                Content = "Emerging technologies are reshaping the way we learn and teach...",
                PublishedDate = DateTime.Now,
                Code = "BLG007"
            },
            new Blog
            {
                Title = "Balancing Work and Online Learning",
                Content = "Many students struggle to balance their professional and academic lives...",
                PublishedDate = DateTime.Now.AddDays(1),
                Code = "BLG008"
            },
            new Blog
            {
                Title = "The Role of AI in Education",
                Content = "Artificial Intelligence is transforming educational experiences...",
                PublishedDate = DateTime.Now.AddDays(2),
                Code = "BLG009"
            },
            new Blog
            {
                Title = "Creating Engaging Online Content",
                Content = "Learn how to create content that keeps students engaged and motivated...",
                PublishedDate = DateTime.Now.AddDays(3),
                Code = "BLG010"
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

        var agencyUser = new IdentityUser(
            Guid.NewGuid(),
            "agency1",
            "agency@onlinecourses.com"
        );

        await _userManager.CreateAsync(agencyUser, "1q2w3E*");
        await _userManager.AddToRoleAsync(agencyUser, "agency");
    }
}