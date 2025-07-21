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
using Acme.OnlineCourses.Agencies;

namespace Acme.OnlineCourses.Reports;

public class ReportAppService : ApplicationService, IReportAppService
{
    private readonly IRepository<Student, Guid> _studentRepository;
    private readonly IRepository<StudentCourse, Guid> _studentCourseRepository;
    private readonly IRepository<Course, Guid> _courseRepository;
    private readonly IRepository<Agency, Guid> _agencyRepository;

    public ReportAppService(
     IRepository<Student, Guid> studentRepository,
     IRepository<StudentCourse, Guid> studentCourseRepository,
     IRepository<Course, Guid> courseRepository,
     IRepository<Agency, Guid> agencyRepository)
    {
        _studentRepository = studentRepository;
        _studentCourseRepository = studentCourseRepository;
        _courseRepository = courseRepository;
        _agencyRepository = agencyRepository;
    }

    public async Task<string> GenerateReportAsync(GenerateReportInput input)
    {
        if (input.ReportType == ReportType.StudentReport)
        {
            //ds dk hoc theo tung thang
            return await ExportStudentReport(input);
        }
        else if (input.ReportType == ReportType.AgencyReport)
        {
            //ds dk hoc theo tung Agency / thang
            return await ExportAgencyReport(input);
        }
        //else if (input.ReportType == ReportType.CourseReport)
        //{
        //    //ds hv hoan thanh khoa hoc theo thang
        //    //not using for now. Dont have completion date
        //    //return await ExportCourseReport(input);
        //}

        // TODO: Implement other report types here
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

    private async Task<string> ExportAgencyReport(GenerateReportInput input)
    {
        // Query students and their courses by agency
        var students = await _studentRepository.GetListAsync();
        var studentCourses = await _studentCourseRepository.GetListAsync();
        var courses = await _courseRepository.GetListAsync();
        var agencies = await _agencyRepository.GetListAsync();

        // Filter by year and month if specified
        var filteredStudentCourses = studentCourses.Where(sc =>
         sc.RegistrationDate.Year == input.Year &&
         (input.Month < 0 || sc.RegistrationDate.Month == input.Month)).ToList();

        // Filter by specific agency if specified
        if (input.AgencyId.HasValue)
        {
            students = students.Where(s => s.AgencyId == input.AgencyId.Value).ToList();
        }

        var data = from s in students
                   join a in agencies on s.AgencyId equals a.Id into ag
                   from agency in ag.DefaultIfEmpty()
                   join sc in filteredStudentCourses on s.Id equals sc.StudentId into scg
                   from sc in scg.DefaultIfEmpty()
                   join c in courses on sc?.CourseId ?? Guid.Empty equals c.Id into cg
                   from c in cg.DefaultIfEmpty()
                   select new
                   {
                       AgencyCode = agency?.Code ?? "N/A",
                       AgencyName = agency?.Name ?? "N/A",
                       StudentName = s.Fullname,
                       Email = s.Email,
                       Phone = s.PhoneNumber,
                       DateOfBirth = s.DateOfBirth,
                       CourseName = c?.Name ?? "N/A",
                       RegistrationDate = sc?.RegistrationDate,
                       ExpectedStudyDate = sc?.ExpectedStudyDate,
                       CourseStatus = sc?.CourseStatus.ToString() ?? "N/A",
                       TestStatus = sc?.TestStatus.ToString() ?? "N/A",
                       PaymentStatus = sc?.PaymentStatus.ToString() ?? "N/A",
                       StudentNote = sc?.StudentNote ?? "",
                       AdminNote = sc?.AdminNote ?? ""
                   };

        // Create Excel file
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Agency Report");

        // Add title
        var titleYear = input.Year.ToString();
        if (input.Month > 0)
        {
            titleYear = $"THÁNG {input.Month}/{input.Year}";
        }
        worksheet.Cell(1, 1).Value = $"BÁO CÁO ĐĂNG KÝ HỌC THEO AGENCY - {titleYear}";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 12).Merge();

        // Add headers
        worksheet.Cell(3, 1).Value = "Mã Agency";
        worksheet.Cell(3, 2).Value = "Tên Agency";
        worksheet.Cell(3, 3).Value = "Tên Học Viên";
        worksheet.Cell(3, 4).Value = "Email";
        worksheet.Cell(3, 5).Value = "Số Điện Thoại";
        worksheet.Cell(3, 6).Value = "Ngày Sinh";
        worksheet.Cell(3, 7).Value = "Tên Khóa Học";
        worksheet.Cell(3, 8).Value = "Ngày Đăng Ký";
        worksheet.Cell(3, 9).Value = "Ngày Dự Kiến Học";
        worksheet.Cell(3, 10).Value = "Trạng Thái Khóa Học";
        worksheet.Cell(3, 11).Value = "Trạng Thái Thi";
        worksheet.Cell(3, 12).Value = "Trạng Thái Thanh Toán";
        worksheet.Cell(3, 13).Value = "Ghi Chú Học Viên";
        worksheet.Cell(3, 14).Value = "Ghi Chú Admin";

        // Style headers
        var headerRange = worksheet.Range(3, 1, 3, 14);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 4;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.AgencyCode;
            worksheet.Cell(row, 2).Value = item.AgencyName;
            worksheet.Cell(row, 3).Value = item.StudentName;
            worksheet.Cell(row, 4).Value = item.Email;
            worksheet.Cell(row, 5).Value = item.Phone;
            worksheet.Cell(row, 6).Value = item.DateOfBirth?.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 7).Value = item.CourseName;
            worksheet.Cell(row, 8).Value = item.RegistrationDate?.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 9).Value = item.ExpectedStudyDate?.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 10).Value = item.CourseStatus;
            worksheet.Cell(row, 11).Value = item.TestStatus;
            worksheet.Cell(row, 12).Value = item.PaymentStatus;
            worksheet.Cell(row, 13).Value = item.StudentNote;
            worksheet.Cell(row, 14).Value = item.AdminNote;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add borders
        var dataRange = worksheet.Range(3, 1, row - 1, 14);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Save to wwwroot/reports
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
        if (!Directory.Exists(reportsDir))
            Directory.CreateDirectory(reportsDir);

        var agencyFilter = input.AgencyId.HasValue ? "_" + input.AgencyId.Value.ToString("N")[..8] : "";
        var fileName = $"AgencyReport_{input.Year}_{agencyFilter}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        var filePath = Path.Combine(reportsDir, fileName);
        workbook.SaveAs(filePath);

        // Return relative path for download
        return $"/reports/{fileName}";
    }

    //ds dk hoc theo tung thang
    private async Task<string> ExportStudentReport(GenerateReportInput input)
    {
        // Query students and their courses
        var students = await _studentRepository.GetListAsync();
        var studentCourses = await _studentCourseRepository.GetListAsync();
        var courses = await _courseRepository.GetListAsync();
        var agencies = await _agencyRepository.GetListAsync();

        // Filter completed courses by year and month
        var filteredStudentCourses = studentCourses.Where(sc =>
         sc.RegistrationDate.Year == input.Year &&
         (input.Month < 0 || sc.RegistrationDate.Month == input.Month)).ToList();

        var data = from s in students
                   join a in agencies on s.AgencyId equals a.Id into ag
                   from agency in ag.DefaultIfEmpty()
                   join sc in filteredStudentCourses on s.Id equals sc.StudentId into scg
                   from sc in scg.DefaultIfEmpty()
                   join c in courses on sc?.CourseId ?? Guid.Empty equals c.Id into cg
                   from c in cg.DefaultIfEmpty()
                   where sc != null // Only include students with registration courses
                   select new
                   {
                       StudentName = s.Fullname,
                       Email = s.Email,
                       Phone = s.PhoneNumber,
                       DateOfBirth = s.DateOfBirth,
                       AgencyCode = agency?.Code ?? "N/A",
                       AgencyName = agency?.Name ?? "N/A",
                       CourseName = c?.Name ?? "N/A",
                       RegistrationDate = sc.RegistrationDate,
                       ExpectedStudyDate = sc.ExpectedStudyDate,
                       //CompletionDate = sc.RegistrationDate, // Assuming completion date is same as registration for now
                       CourseStatus = sc.CourseStatus.ToString(),
                       TestStatus = sc.TestStatus.ToString(),
                       PaymentStatus = sc.PaymentStatus.ToString(),
                       StudentNote = sc.StudentNote ?? "",
                       AdminNote = sc.AdminNote ?? ""
                   };

        // Create Excel file
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Registration Students");

        // Add title
        var titleYear = input.Year.ToString();
        if (input.Month > 0)
        {
            titleYear = $"THÁNG {input.Month}/{input.Year}";
        }
        worksheet.Cell(1, 1).Value = $"BÁO CÁO HỌC VIÊN ĐĂNG KÍ KHÓA HỌC - {titleYear}";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 15).Merge();

        // Add headers
        worksheet.Cell(3, 1).Value = "Tên Học Viên";
        worksheet.Cell(3, 2).Value = "Email";
        worksheet.Cell(3, 3).Value = "Số Điện Thoại";
        worksheet.Cell(3, 4).Value = "Ngày Sinh";
        worksheet.Cell(3, 5).Value = "Mã Agency";
        worksheet.Cell(3, 6).Value = "Tên Agency";
        worksheet.Cell(3, 7).Value = "Tên Khóa Học";
        worksheet.Cell(3, 8).Value = "Ngày Đăng Ký";
        worksheet.Cell(3, 9).Value = "Ngày Dự Kiến Học";
        worksheet.Cell(3, 10).Value = "Trạng Thái Khóa Học";
        worksheet.Cell(3, 11).Value = "Trạng Thái Thi";
        worksheet.Cell(3, 12).Value = "Trạng Thái Thanh Toán";
        worksheet.Cell(3, 13).Value = "Ghi Chú Học Viên";
        worksheet.Cell(3, 14).Value = "Ghi Chú Admin";

        // Style headers
        var headerRange = worksheet.Range(3, 1, 3, 15);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 4;
        foreach (var item in data)
        {
            worksheet.Cell(row, 1).Value = item.StudentName;
            worksheet.Cell(row, 2).Value = item.Email;
            worksheet.Cell(row, 3).Value = item.Phone;
            worksheet.Cell(row, 4).Value = item.DateOfBirth?.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 5).Value = item.AgencyCode;
            worksheet.Cell(row, 6).Value = item.AgencyName;
            worksheet.Cell(row, 7).Value = item.CourseName;
            worksheet.Cell(row, 8).Value = item.RegistrationDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 9).Value = item.ExpectedStudyDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 10).Value = item.CourseStatus;
            worksheet.Cell(row, 11).Value = item.TestStatus;
            worksheet.Cell(row, 12).Value = item.PaymentStatus;
            worksheet.Cell(row, 13).Value = item.StudentNote;
            worksheet.Cell(row, 14).Value = item.AdminNote;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add borders
        var dataRange = worksheet.Range(3, 1, row - 1, 15);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Add summary statistics
        var totalStudents = data.Count();
        worksheet.Cell(row + 2, 1).Value = "TỔNG KẾT:";
        worksheet.Cell(row + 2, 1).Style.Font.Bold = true;
        worksheet.Cell(row + 3, 1).Value = $"Tổng số học viên đăng kí khóa học trong {titleYear.ToLower()}: {totalStudents}";

        // Save to wwwroot/reports
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
        if (!Directory.Exists(reportsDir))
            Directory.CreateDirectory(reportsDir);
        var fileName = $"StudentReport_Registration_{input.Year}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        var filePath = Path.Combine(reportsDir, fileName);
        workbook.SaveAs(filePath);

        // Return relative path for download
        return $"/reports/{fileName}";
    }

    //hv da hoan thanh khoa hoc theo tung thang
    private async Task<string> ExportCourseReport(GenerateReportInput input)
    {
        var students = await _studentRepository.GetListAsync();
        var studentCourses = await _studentCourseRepository.GetListAsync();
        var courses = await _courseRepository.GetListAsync();
        var agencies = await _agencyRepository.GetListAsync();

        // Lọc các đăng ký đã hoàn thành theo năm/tháng
        var filteredStudentCourses = studentCourses.Where(sc =>
         sc.CourseStatus == StudentCourseStatus.Completed &&
         sc.RegistrationDate.Year == input.Year &&
         (input.Month < 0 || sc.RegistrationDate.Month == input.Month)).ToList();//todo? completed day

        var data = from sc in filteredStudentCourses
                   join s in students on sc.StudentId equals s.Id
                   join c in courses on sc.CourseId equals c.Id
                   join a in agencies on s.AgencyId equals a.Id into ag
                   from agency in ag.DefaultIfEmpty()
                   select new
                   {
                       CourseName = c.Name,
                       StudentName = s.Fullname,
                       Email = s.Email,
                       Phone = s.PhoneNumber,
                       DateOfBirth = s.DateOfBirth,
                       AgencyCode = agency?.Code ?? "N/A",
                       AgencyName = agency?.Name ?? "N/A",
                       RegistrationDate = sc.RegistrationDate,
                       ExpectedStudyDate = sc.ExpectedStudyDate,
                       CompletionDate = sc.RegistrationDate, // Nếu có trường hoàn thành thì thay thế
                       CourseStatus = sc.CourseStatus.ToString(),
                       TestStatus = sc.TestStatus.ToString(),
                       PaymentStatus = sc.PaymentStatus.ToString(),
                       StudentNote = sc.StudentNote ?? "",
                       AdminNote = sc.AdminNote ?? ""
                   };

        // Tạo file Excel
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Course Report");

        // Tiêu đề
        worksheet.Cell(1, 1).Value = $"BÁO CÁO HỌC VIÊN HOÀN THÀNH THEO KHÓA HỌC - THÁNG {input.Month}/{input.Year}";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 16).Merge();

        // Header
        worksheet.Cell(3, 1).Value = "Tên Khóa Học";
        worksheet.Cell(3, 2).Value = "Tên Học Viên";
        worksheet.Cell(3, 3).Value = "Email";
        worksheet.Cell(3, 4).Value = "Số Điện Thoại";
        worksheet.Cell(3, 5).Value = "Ngày Sinh";
        worksheet.Cell(3, 6).Value = "Mã Agency";
        worksheet.Cell(3, 7).Value = "Tên Agency";
        worksheet.Cell(3, 8).Value = "Ngày Đăng Ký";
        worksheet.Cell(3, 9).Value = "Ngày Dự Kiến Học";
        worksheet.Cell(3, 10).Value = "Ngày Hoàn Thành";
        worksheet.Cell(3, 11).Value = "Trạng Thái Khóa Học";
        worksheet.Cell(3, 12).Value = "Trạng Thái Thi";
        worksheet.Cell(3, 13).Value = "Trạng Thái Thanh Toán";
        worksheet.Cell(3, 14).Value = "Ghi Chú Học Viên";
        worksheet.Cell(3, 15).Value = "Ghi Chú Admin";

        // Style header
        var headerRange = worksheet.Range(3, 1, 3, 15);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 4;
        foreach (var item in data.OrderBy(d => d.CourseName).ThenBy(d => d.StudentName))
        {
            worksheet.Cell(row, 1).Value = item.CourseName;
            worksheet.Cell(row, 2).Value = item.StudentName;
            worksheet.Cell(row, 3).Value = item.Email;
            worksheet.Cell(row, 4).Value = item.Phone;
            worksheet.Cell(row, 5).Value = item.DateOfBirth?.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 6).Value = item.AgencyCode;
            worksheet.Cell(row, 7).Value = item.AgencyName;
            worksheet.Cell(row, 8).Value = item.RegistrationDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 9).Value = item.ExpectedStudyDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 10).Value = item.CompletionDate.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 11).Value = item.CourseStatus;
            worksheet.Cell(row, 12).Value = item.TestStatus;
            worksheet.Cell(row, 13).Value = item.PaymentStatus;
            worksheet.Cell(row, 14).Value = item.StudentNote;
            worksheet.Cell(row, 15).Value = item.AdminNote;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add borders
        var dataRange = worksheet.Range(3, 1, row - 1, 15);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Tổng kết
        var total = data.Count();
        worksheet.Cell(row + 2, 1).Value = "TỔNG KẾT:";
        worksheet.Cell(row + 2, 1).Style.Font.Bold = true;
        worksheet.Cell(row + 3, 1).Value = $"Tổng số học viên hoàn thành các khóa học trong tháng {input.Month}/{input.Year}: {total}";

        // Lưu file
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
        if (!Directory.Exists(reportsDir))
            Directory.CreateDirectory(reportsDir);
        var fileName = $"CourseReport_{input.Year}_{input.Month:00}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        var filePath = Path.Combine(reportsDir, fileName);
        workbook.SaveAs(filePath);

        // Trả về đường dẫn file
        return $"/reports/{fileName}";
    }
}