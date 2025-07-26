namespace Acme.OnlineCourses.Helpers;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
    Task SendWelcomeEmailAsync(WelcomeRequest request);
    Task SendWelcomePartnerEmailAsync(WelcomeRequest request);
    Task SendNotifyToAdminsAsync(NotityToAdminRequest request);
    Task SendNotifyUpdateAttachmentAsync(NotifyUpdateAttachmentRequest request);
    Task SendResetPasswordAsync(ResetPasswordRequest request);
}