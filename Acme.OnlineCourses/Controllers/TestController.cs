using Acme.OnlineCourses.Blogs;
using Acme.OnlineCourses.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Acme.OnlineCourses.Controllers
{
    public class TestController : Controller
    {
        private readonly IMailService mailService;
        public TestController(IMailService mailService)
        {
            this.mailService = mailService;

            //WelcomeRequest request = new WelcomeRequest();
            //request.ToEmail = "fcmtuan@gmail.com";
            //request.UserName = "Chao Tu An";

            //try
            //{
            //    mailService.SendWelcomeEmailAsync(request);
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            var test = PasswordGenerator.GenerateSecurePassword(8);
        }

        public IActionResult Index()
        {
           
            return View();
        }
    }


}
