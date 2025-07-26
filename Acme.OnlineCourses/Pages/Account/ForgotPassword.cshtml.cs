using Acme.OnlineCourses.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.OnlineCourses.Pages.Account;

public class ForgotPasswordModel : AbpPageModel
{
    private readonly IMailService _mailService;
    private readonly UserManager<Volo.Abp.Identity.IdentityUser> _userManager;

    public ForgotPasswordModel(IMailService mailService, UserManager<Volo.Abp.Identity.IdentityUser> userManager)
    {
        _mailService = mailService;
        _userManager = userManager;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    public string SuccessMessage { get; set; }
    public string ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (!ModelState.IsValid)
        {
            // You may want to localize validation messages in the view model attributes.
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Email);
        if (user == null)
        {
            ErrorMessage = currentCulture == "vi"
                ? "Không tìm thấy người dùng với email này."
                : "No user found with this email.";
            return Page();
        }

        var newPassword = PasswordGenerator.GenerateSecurePassword(10);

        // Remove old password and set new one
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            return Page();
        }

        // Send email with new password
        await _mailService.SendResetPasswordAsync(new ResetPasswordRequest
        {
            ToEmail = Email,
            NewPassword = newPassword,
        });

        SuccessMessage = currentCulture == "vi"
            ? "Mật khẩu mới đã được gửi tới email của bạn."
            : "A new password has been sent to your email.";
        ModelState.Clear();

        return Page();
    }
}