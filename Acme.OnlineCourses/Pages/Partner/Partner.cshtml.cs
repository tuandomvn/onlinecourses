using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages.Partner;

public class PartnerModel : AbpPageModel
{
    public async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}
