using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Helpers;
using Acme.OnlineCourses.Students;
using Microsoft.AspNetCore.Mvc;

namespace Acme.OnlineCourses.Controllers
{
    public class TestController : Controller
    {
        private readonly IMailService mailService;
        public TestController(IMailService mailService)
        {
            this.mailService = mailService;

            NotityToAdminRequest request = new NotityToAdminRequest();
            request.ToEmail = new List<string> { "fcmtuan@gmail.com", "tuandomvn@gmail.com" };
            request.StudentName = "student.Fullname";
            request.StudentEmail = "student.Email";
            request.CourseName = "CourseName";
            try
            {
                mailService.SendNotifyToAdminsAsync(request);
            }
            catch (Exception ex)
            {

                throw;
            }

            var test = PasswordGenerator.GenerateSecurePassword(8);
        }

        public IActionResult Index()
        {

            return View();
        }
    }


}