using System.Web.Mvc;

namespace OnlineLib.App.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        [Route("~/")]
        public ActionResult Index()
        {
            return View();
        }


        [Route("~/Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}