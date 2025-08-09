using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Identity;

namespace Acme.OnlineCourses.Pages.Account;
public class ChangePasswordModel : PageModel
{
    private readonly UserManager<Volo.Abp.Identity.IdentityUser> _userManager;
    private readonly ICurrentUser _currentUser;
    private string _currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    public ChangePasswordModel(
        ICurrentUser currentUser,
        UserManager<Volo.Abp.Identity.IdentityUser> userManager)
    {
        _userManager = userManager;
        _currentUser = currentUser;
        InstructionMessage = _currentCulture == "vi"
            ? "Mật khẩu bao gồm kí tự và chữ số, ít nhất 8 kí tự."
            : "Password must contain letters and numbers, minimum 8 characters.";
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string SuccessMessage { get; set; }
    public string InstructionMessage { get; set; }
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
            ErrorMessage = _currentCulture == "vi"
                ? "Vui lòng nhập đầy đủ thông tin."
                : "Please enter all required information.";
            return Page();
        }

        if (Input.NewPassword != Input.ConfirmPassword)
        {
            ErrorMessage = _currentCulture == "vi"
                ? "Mật khẩu mới và xác nhận mật khẩu không khớp."
                : "New password and confirmation do not match.";
            return Page();
        }

        var user = await _userManager.FindByIdAsync(_currentUser.Id.ToString());
        if (user == null)
        {
            ErrorMessage = _currentCulture == "vi"
                ? "Không tìm thấy người dùng."
                : "User not found.";
            return Page();
        }

        var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);

        if (result.Succeeded)
        {
            SuccessMessage = _currentCulture == "vi"
                ? "Đổi mật khẩu thành công."
                : "Password changed successfully.";
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