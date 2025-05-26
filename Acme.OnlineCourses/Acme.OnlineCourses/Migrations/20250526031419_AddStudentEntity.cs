using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Acme.OnlineCourses.Migrations;

public partial class AddStudentEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Students",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                FullName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PhoneNumber = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                IdentityNumber = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                CourseName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                TestStatus = table.Column<int>(type: "int", nullable: false),
                PaymentStatus = table.Column<int>(type: "int", nullable: false),
                AccountStatus = table.Column<int>(type: "int", nullable: false),
                InternalNote = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                AgencyId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                AssignedAdminId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                PaymentProofFile = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Students", x => x.Id);
                table.ForeignKey(
                    name: "FK_Students_Agencies_AgencyId",
                    column: x => x.AgencyId,
                    principalTable: "Agencies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Students_AbpUsers_AssignedAdminId",
                    column: x => x.AssignedAdminId,
                    principalTable: "AbpUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_Students_AgencyId",
            table: "Students",
            column: "AgencyId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_AssignedAdminId",
            table: "Students",
            column: "AssignedAdminId");

        migrationBuilder.CreateIndex(
            name: "IX_Students_Email",
            table: "Students",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "IX_Students_PhoneNumber",
            table: "Students",
            column: "PhoneNumber");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Students");
    }
} 