using System;

namespace Acme.OnlineCourses.Reports;

public class GenerateReportInput
{
    public int Year { get; set; }
    public int Month { get; set; }
    public ReportType ReportType { get; set; }
    public Guid? AgencyId { get; set; }
} 