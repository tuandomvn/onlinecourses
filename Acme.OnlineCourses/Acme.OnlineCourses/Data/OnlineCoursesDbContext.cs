using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Acme.OnlineCourses.Entities;

namespace Acme.OnlineCourses.Data;

[ConnectionStringName("Default")]
public class OnlineCoursesDbContext : AbpDbContext<OnlineCoursesDbContext>
{
    public OnlineCoursesDbContext(DbContextOptions<OnlineCoursesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Agency> Agencies { get; set; } = default!;
    public DbSet<Student> Students { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureFeatureManagement();

        builder.Entity<Agency>(b =>
        {
            b.HasIndex(x => x.AgencyCode).IsUnique();
            b.Property(x => x.AgencyCode).IsRequired().HasMaxLength(32);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.ContactEmail).HasMaxLength(128);
            b.Property(x => x.ContactPhone).HasMaxLength(32);
            b.Property(x => x.CommissionPercent).IsRequired();
            b.Property(x => x.Status).IsRequired();
        });

        builder.Entity<Student>(b =>
        {
            b.HasIndex(x => x.Email).IsUnique();
            b.HasIndex(x => x.PhoneNumber).IsUnique();
            b.Property(x => x.FullName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Email).IsRequired().HasMaxLength(256);
            b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(32);
            b.Property(x => x.IdentityNumber).HasMaxLength(32);
            b.Property(x => x.CourseName).IsRequired().HasMaxLength(256);
            b.Property(x => x.InternalNote).HasMaxLength(1024);
            b.Property(x => x.PaymentProofFile).HasMaxLength(1024);
            
            b.HasOne(x => x.Agency)
                .WithMany()
                .HasForeignKey(x => x.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.AssignedAdmin)
                .WithMany()
                .HasForeignKey(x => x.AssignedAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
