using Acme.OnlineCourses.Students;
using AutoMapper.Internal;
using DocumentFormat.OpenXml.Office2016.Excel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Acme.OnlineCourses.Helpers
{
    public class WelcomeRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CourseName { get; set; }
    }

    public class RegistationInstructionRequest
    {
        public string ToEmail { get; set; }
        public string Language { get; set; }
    }

    public class NotityToAdminRequest
    {
        public List<string> ToEmail { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
    }

    public class NotityNewPartnerToAdminRequest
    {
        public List<string> ToEmail { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class NotifyUpdateAttachmentRequest
    {
        public List<string> ToEmail { get; set; }
        public string StudentEmail { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string ToEmail { get; set; }
        public string NewPassword { get; set; }
    }

    public class MailRequest
    {
        public List<string> ToEmail { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;
        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
        {
            _logger = logger;
            _mailSettings = mailSettings.Value;
        }

        //This is for test only
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);

            // Add all recipients to the To field
            foreach (var recipient in mailRequest.ToEmail)
            {
                email.To.Add(MailboxAddress.Parse(recipient));
            }

            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        //public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        //{
        //    string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
        //    StreamReader str = new StreamReader(FilePath);
        //    string MailText = str.ReadToEnd();
        //    str.Close();

        //    MailText = MailText
        //        .Replace("[username]", request.UserName)
        //        .Replace("[email]", request.ToEmail)
        //        .Replace("[password]", request.Password)
        //        .Replace("[coursename]", request.CourseName);

        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        //    email.To.Add(MailboxAddress.Parse(request.ToEmail));
        //    email.Subject = $"[TESOL Channel] - Chào mừng học viên {request.UserName}";
        //    var builder = new BodyBuilder();
        //    builder.HtmlBody = MailText;
        //    email.Body = builder.ToMessageBody();
        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    await smtp.SendAsync(email);
        //    smtp.Disconnect(true);
        //}

        public async Task SendRegistationInstructionEmailAsync(RegistationInstructionRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + $"\\Templates\\RegistrationInstruction.{request.Language}.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();


                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = request.Language == "en" ?
                    "INSTRUCTIONS TO COMPLETE YOUR TESOL INTERNATIONAL CERTIFICATE COURSE REGISTRATION (LTi AUSTRALIA)" :
                    "HƯỚNG DẪN HOÀN THÀNH ĐĂNG KÝ KHOÁ HỌC TESOL CHỨNG CHỈ QUỐC TẾ (LTi ÚC)";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Registration instruction email sent to {request.ToEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send registration instruction email to {request.ToEmail}: {ex.Message}");
            }
        }

        public async Task SendWelcomePartnerEmailAsync(WelcomeRequest request, bool isActive)
        {
            try
            {
                string filePath = string.Empty;
                if (isActive)
                {
                    filePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomePartnerTemplate.html";
                }
                else
                {
                    filePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomePartnerTemplate_Inactive.html";
                }

                StreamReader str = new StreamReader(filePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[username]", request.UserName)
                    .Replace("[email]", request.ToEmail)
                    .Replace("[password]", request.Password);

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = $"[TESOL Channel] - Chào mừng đại lý {request.UserName}";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Welcome partner email sent to {request.ToEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send welcome partner email to {request.ToEmail}: {ex.Message}");
            }
        }
        public async Task SendNotifyToAdminsAsync(NotityToAdminRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\Noti.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[coursename]", request.CourseName)
                    .Replace("[studentname]", request.StudentEmail);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };
                foreach (var recipient in request.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }
                email.Subject = $"[TESOL Channel] - Thông báo học viên {request.StudentName} đăng ký khóa học";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Notification email sent to admins for new student {request.StudentEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send notification email to admins for new student {request.StudentEmail}: {ex.Message}");
            }
        }
        public async Task SendNotifyNewPartnerToAdminsAsync(NotityNewPartnerToAdminRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\NotiPartner.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[name]", request.Name)
                    .Replace("[email]", request.Email);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };
                foreach (var recipient in request.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }
                email.Subject = $"[TESOL Channel] - Thông báo đăng ký đối tác - {request.Name}";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Notification email sent to admins for new partner {request.Name}");
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to send notification email to admins for new partner {request.Name}");
            }
        }

        public async Task SendJobNotiToAdminsAsync(NotityNewPartnerToAdminRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\JobSupport.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[name]", request.Name)
                    .Replace("[email]", request.Email);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };
                foreach (var recipient in request.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }
                email.Subject = $"[TESOL Channel] - Thông báo hỗ trợ việc làm - {request.Name}";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Job support notification email sent to admins for email {request.Email}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send job support notification email to admins for email {request.Email} {ex.Message}");
            }
        }

        public async Task SendNotifyUpdateAttachmentAsync(NotifyUpdateAttachmentRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\UpdateAttachment.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[studentEmail]", request.StudentEmail);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };
                foreach (var recipient in request.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }
                email.Subject = $"[TESOL Channel] - Thông báo học viên {request.StudentEmail} đã cập nhật tài liệu";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                _logger.LogInformation($"Update attachment notification email sent to admins for student {request.StudentEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send update attachment notification email to admins for student {request.StudentEmail} {ex.Message}");
            }
           
        }

        public async Task SendResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\NewPassword.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();

                MailText = MailText
                    .Replace("[newpassword]", request.NewPassword);

                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = "[TESOL Channel] - Đặt lại password thành công";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send reset password email to {request.ToEmail}: {ex.Message}");
            }
        }
    }
}
