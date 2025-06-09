using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.OnlineCourses.Reports;

public interface IReportAppService : IApplicationService
{
    Task<string> GenerateReportAsync(GenerateReportInput input);
} 