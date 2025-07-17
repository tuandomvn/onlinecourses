using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Identity;

namespace Acme.OnlineCourses.Pages.Account;
public class ChangePasswordModel : PageModel
{
    private readonly UserManager<Volo.Abp.Identity.IdentityUser> _userManager;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordModel(
        ICurrentUser currentUser,
        UserManager<Volo.Abp.Identity.IdentityUser> userManager)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string SuccessMessage { get; set; }
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
            return Page();
        }

        if (Input.NewPassword != Input.ConfirmPassword)
        {
            ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.";
            return Page();
        }

        var user = await _userManager.FindByIdAsync(_currentUser.Id.ToString());
        if (user == null)
        {
            ErrorMessage = "Không tìm thấy người dùng.";
            return Page();
        }

        var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);

        if (result.Succeeded)
        {
            SuccessMessage = "Đổi mật khẩu thành công.";
            ModelState.Clear();
            return Page();
        }
        else
        {
            ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            return Page();
        }
    }
}