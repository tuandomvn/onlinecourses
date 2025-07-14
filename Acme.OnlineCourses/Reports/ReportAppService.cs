using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using ClosedXML.Excel;
using Acme.OnlineCourses.Students;
using Acme.OnlineCourses.Students.Dtos;
using Volo.Abp.Domain.Repositories;
using System.IO;
using System.Linq;
using Acme.OnlineCourses.Courses;

namespace Acme.OnlineCourses.Reports;

public class ReportAppService : ApplicationService, IReportAppService
{
    private readonly IRepository<Student, Guid> _studentRepository;
    private readonly IRepository<StudentCourse, Guid> _studentCourseRepository;
    private readonly IRepository<Course, Guid> _courseRepository;

    public ReportAppService(
        IRepository<Student, Guid> studentRepository,
        IRepository<StudentCourse, Guid> studentCourseRepository,
        IRepository<Course, Guid> courseRepository)
    {
        _studentRepository = studentRepository;
        _studentCourseRepository = studentCourseRepository;
        _courseRepository = courseRepository;
    }

    public async Task<string> GenerateReportAsync(GenerateReportInput input)
    {
        if (input.ReportType == ReportType.StudentReport)
        {
            // Query students and their courses
            var students = await _studentRepository.GetListAsync();
            var studentCourses = await _studentCourseRepository.GetListAsync();
            var courses = await _courseRepository.GetListAsync();

            var data = from s in students
                       join sc in studentCourses on s.Id equals sc.StudentId into scg
                       from sc in scg.DefaultIfEmpty()
                       join c in courses on sc?.CourseId ?? Guid.Empty equals c.Id into cg
                       from c in cg.DefaultIfEmpty()
                       select new
                       {
                           StudentName = s.Fullname,
                           Email = s.Email,
                           Phone = s.PhoneNumber,
                           DateOfBirth = s.DateOfBirth,
                           CourseName = c?.Name,
                           RegistrationDate = sc?.RegistrationDate,
                           CourseStatus = sc?.CourseStatus.ToString(),
                           TestStatus = sc?.TestStatus.ToString(),
                           PaymentStatus = sc?.PaymentStatus.ToString(),
                           StudentNote = sc?.StudentNote
                       };

            // Create Excel file
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Students");
            worksheet.Cell(1, 1).Value = "Student Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Phone";
            worksheet.Cell(1, 4).Value = "Date of Birth";
            worksheet.Cell(1, 5).Value = "Course Name";
            worksheet.Cell(1, 6).Value = "Registration Date";
            worksheet.Cell(1, 7).Value = "Course Status";
            worksheet.Cell(1, 8).Value = "Test Status";
            worksheet.Cell(1, 9).Value = "Payment Status";
            worksheet.Cell(1, 10).Value = "Student Note";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.StudentName;
                worksheet.Cell(row, 2).Value = item.Email;
                worksheet.Cell(row, 3).Value = item.Phone;
                worksheet.Cell(row, 4).Value = item.DateOfBirth?.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 5).Value = item.CourseName;
                worksheet.Cell(row, 6).Value = item.RegistrationDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 7).Value = item.CourseStatus;
                worksheet.Cell(row, 8).Value = item.TestStatus;
                worksheet.Cell(row, 9).Value = item.PaymentStatus;
                worksheet.Cell(row, 10).Value = item.StudentNote;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            // Save to wwwroot/reports
            var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
            if (!Directory.Exists(reportsDir))
                Directory.CreateDirectory(reportsDir);
            var fileName = $"StudentReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            var filePath = Path.Combine(reportsDir, fileName);
            workbook.SaveAs(filePath);

            // Return relative path for download
            return $"/reports/{fileName}";
        }

        // TODO: Implement your report generation logic here
        // This is a sample implementation
        var reportHtml = $@"
            <div class='report-container'>
                <h2>Report for {input.Month}/{input.Year}</h2>
                <div class='report-content'>
                    <p>This is a sample report for {input.Month}/{input.Year}</p>
                    <!-- Add your report content here -->
                </div>
            </div>";

        return reportHtml;
    }
} 