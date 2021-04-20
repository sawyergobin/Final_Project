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
        public ActionResult Contact(int UPDATEMEEEEE)
        {
            string emailServer = WebConfigurationManager.AppSettings["EmailServer"];            string emailPW = WebConfigurationManager.AppSettings["EmailPW"];            string emailUser = WebConfigurationManager.AppSettings["EmailUser"];            string emailToAddress = WebConfigurationManager.AppSettings["EmailToAddress"];

            return View();
        }
    }
}
