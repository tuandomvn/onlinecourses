using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Data;
using Acme.OnlineCourses.Students;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.OnlineCourses.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class OnlineCoursesDbContext : AbpDbContext<OnlineCoursesDbContext>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Agency> Agencies { get; set; }
    public DbSet<Blog> Blogs { get; set; }

    public OnlineCoursesDbContext(DbContextOptions<OnlineCoursesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureOnlineCourses();
    }
} 