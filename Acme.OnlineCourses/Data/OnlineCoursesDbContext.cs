using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses.Students;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Acme.OnlineCourses.Data;

public class OnlineCoursesDbContext : AbpDbContext<OnlineCoursesDbContext>
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Agency> Agencies { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<EmploymentSupport> EmploymentSupports { get; set; }

    public OnlineCoursesDbContext(DbContextOptions<OnlineCoursesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        //builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.ConfigureOnlineCourses();
    }
}
