using FinalProject.UI.MVC.Models;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string emailServer = WebConfigurationManager.AppSettings["EmailServer"];
            string emailPW = WebConfigurationManager.AppSettings["EmailPW"];
            string emailUser = WebConfigurationManager.AppSettings["EmailUser"];
            string emailToAddress = WebConfigurationManager.AppSettings["EmailToAddress"];
            
            // 1. build the body of the email
            string body = $"You have revieved an email from {cvm.Name} at the address {cvm.Email} with a subject of {cvm.Subject}<br/><br/><strong>Message:</strong>{cvm.Message}";

            // 2.instantiate the MainMessage object (needs System.Net.Mail)
            MailMessage msg = new MailMessage(emailUser, emailToAddress, cvm.Subject, body);
            // (optional) customize the MainMessage properties
            msg.IsBodyHtml = true;

            //instantiate the SmtpClient
            SmtpClient client = new SmtpClient(emailServer);

            // 4. provide credentials for the client (needs the System.Net namespace)
            client.Credentials = new NetworkCredential(emailUser, emailPW);

            // 5. attempt to send the email
            try
            {
                client.Send(msg);
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Sorry, the mail server had some trouble with that request. See the stacktrace or contact an admin." + ex.StackTrace;
            }
            return View("EmailConfirmation", cvm);
            
        }
    }
}
