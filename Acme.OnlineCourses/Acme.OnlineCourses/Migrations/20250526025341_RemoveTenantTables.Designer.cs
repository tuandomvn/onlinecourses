using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Acme.OnlineCourses.Data;

namespace Acme.OnlineCourses.Migrations;

[DbContext(typeof(OnlineCoursesDbContext))]
[Migration("20250526025341_RemoveTenantTables")]
public partial class RemoveTenantTablesModelSnapshot_Old : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 64);

        modelBuilder.Entity("Acme.OnlineCourses.OnlineCoursesDbContextModelSnapshot+ModelSnapshot", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            b.HasKey("Id");

            b.ToTable("ModelSnapshot");
        });
    }
} 