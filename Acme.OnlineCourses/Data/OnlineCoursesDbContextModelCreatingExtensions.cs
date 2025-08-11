using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Courses;
using Acme.OnlineCourses.EmpSupport;
using Acme.OnlineCourses.Students;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Acme.OnlineCourses.Data;

public static class OnlineCoursesDbContextModelCreatingExtensions
{
    public static void ConfigureOnlineCourses(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Blog>(b =>
        {
            b.ToTable("Blogs", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Summary)
                .HasMaxLength(1024);

            b.Property(x => x.Content)
                .IsRequired();
        });

        builder.Entity<Student>(b =>
        {
            b.ToTable("Students", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Fullname)
                .IsRequired()
                .HasMaxLength(500);

            b.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.PhoneNumber)
                .HasMaxLength(32);

            b.HasIndex(x => x.Email);
        });

        builder.Entity<StudentCourse>(b =>
        {
            b.ToTable("StudentCourse", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasOne<Student>()
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.StudentId)
                .IsRequired();
        });

        builder.Entity<StudentAttachment>(b =>
        {
            b.ToTable("StudentAttachment", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(512);

            b.HasOne<Student>()
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.StudentId)
                .IsRequired();
        });

        builder.Entity<Agency>(b =>
        {
            b.ToTable("Agencies", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(32);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Description)
                .HasMaxLength(1024);

            b.Property(x => x.ContactEmail)
                .HasMaxLength(256);

            b.Property(x => x.ContactPhone)
                .HasMaxLength(32);

            b.Property(x => x.Address)
                .HasMaxLength(512);

            b.HasIndex(x => x.Code);
            b.HasIndex(x => x.Name);
        });

        builder.Entity<Course>(b =>
        {
            b.ToTable("Courses", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).IsRequired().HasMaxLength(32);
            b.Property(x => x.Name).IsRequired().HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(1024);
            b.Property(x => x.Price).HasPrecision(10, 2);
        });

        builder.Entity<EmploymentSupport>(b =>
        {
            b.ToTable("EmploymentSupports", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FullName).IsRequired().HasMaxLength(256);
            b.Property(x => x.DateOfBirth).IsRequired();
            b.Property(x => x.PhoneNumber).HasMaxLength(32);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Address).HasMaxLength(512);
            b.Property(x => x.CourseCompletionDate).IsRequired();
            b.Property(x => x.Message).HasMaxLength(1024);
        });
    }
} 