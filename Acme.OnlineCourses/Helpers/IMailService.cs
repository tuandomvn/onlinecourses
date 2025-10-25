namespace Acme.OnlineCourses.Helpers;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
    //Task SendWelcomeEmailAsync(WelcomeRequest request);

    Task SendRegistationInstructionEmailAsync(RegistationInstructionRequest request);

    Task SendWelcomePartnerEmailAsync(WelcomeRequest request, bool isActive);
    Task SendNotifyToAdminsAsync(NotityToAdminRequest request);
    Task SendNotifyUpdateAttachmentAsync(NotifyUpdateAttachmentRequest request);
    Task SendResetPasswordAsync(ResetPasswordRequest request);
    Task SendNotifyNewPartnerToAdminsAsync(NotityNewPartnerToAdminRequest request);
    Task SendJobNotiToAdminsAsync(NotityNewPartnerToAdminRequest request);
}