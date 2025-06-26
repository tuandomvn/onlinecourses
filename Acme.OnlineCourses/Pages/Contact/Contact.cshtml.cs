using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages.Contact;

public class ContactModel : AbpPageModel
{
    public async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}