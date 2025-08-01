using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Acme.OnlineCourses.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Reports;
[Authorize(Roles = OnlineCoursesConsts.Roles.Administrator)]
public class IndexModel : PageModel
{
    private readonly IReportAppService _reportAppService;
    private readonly IAgencyAppService _agencyAppService;

    public IndexModel(
        IReportAppService reportAppService,
        IAgencyAppService agencyAppService)
    {
        _reportAppService = reportAppService;
        _agencyAppService = agencyAppService;
    }

    [BindProperty]
    public ReportType SelectedReportType { get; set; }

    [BindProperty]
    public int SelectedYear { get; set; }

    [BindProperty]
    public int SelectedMonth { get; set; }

    [BindProperty]
    public Guid? SelectedAgencyId { get; set; }

    public List<AgencyDto> Agencies { get; set; }

    public string ReportContent { get; set; }

    public async Task OnGetAsync()
    {
        // Set default values
        SelectedReportType = ReportType.StudentReport;
        SelectedYear = DateTime.Now.Year;
        SelectedMonth = DateTime.Now.Month;

        // Load agencies
        var agenciesResult = await _agencyAppService.GetListAsync(new PagedAndSortedResultRequestDto
        {
            MaxResultCount = 1000 // Adjust as needed
        });
        Agencies = agenciesResult.Items.Where(e=>e.Status == AgencyStatus.Active) .ToList();
    }

    public async Task<IActionResult> OnPostGenerateReportAsync()
    {
        try
        {
            var input = new GenerateReportInput
            {
                ReportType = SelectedReportType,
                Year = SelectedYear,
                Month = SelectedMonth,
                AgencyId = SelectedReportType == ReportType.AgencyReport ? SelectedAgencyId : null
            };

            var result = await _reportAppService.GenerateReportAsync(input);

            // Nếu là file Excel, trả về file download
            if (!string.IsNullOrEmpty(result) && result.StartsWith("/reports/") && result.EndsWith(".xlsx"))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", result.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                var fileName = Path.GetFileName(filePath);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            ReportContent = result;

            // Reload agencies for the view
            var agenciesResult = await _agencyAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 1000 // Adjust as needed
            });
            Agencies = agenciesResult.Items.ToList();

            return Page();
        }
        catch (Exception ex)
        {
            // You might want to add proper error handling here
            ModelState.AddModelError(string.Empty, "Error generating report");
            return Page();
        }
    }
} 