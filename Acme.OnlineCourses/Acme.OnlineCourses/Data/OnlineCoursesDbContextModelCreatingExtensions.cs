using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Blogs;
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

            b.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Summary)
                .HasMaxLength(1024);

            b.Property(x => x.Content)
                .IsRequired();

            b.Property(x => x.FeaturedImage)
                .HasMaxLength(1024);

            b.Property(x => x.Author)
                .HasMaxLength(128);

            b.Property(x => x.Tags)
                .HasMaxLength(512);

            b.HasIndex(x => x.Slug);
        });

        builder.Entity<Student>(b =>
        {
            b.ToTable("Students", OnlineCoursesConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(128);

            b.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(128);

            b.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.PhoneNumber)
                .HasMaxLength(32);

            b.HasIndex(x => x.Email);
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
    }
} 