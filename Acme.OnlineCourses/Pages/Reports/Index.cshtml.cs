using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Acme.OnlineCourses.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using Acme.OnlineCourses.Agencies;
using Acme.OnlineCourses.Agencies.Dtos;
using Volo.Abp.Application.Dtos;

namespace Acme.OnlineCourses.Pages.Reports;

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
        Agencies = agenciesResult.Items.ToList();
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

            ReportContent = await _reportAppService.GenerateReportAsync(input);

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