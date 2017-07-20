using System.Web.Mvc;

namespace PublicApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Analog Shiftplanning Public API";

            return View();
        }
    }
}
