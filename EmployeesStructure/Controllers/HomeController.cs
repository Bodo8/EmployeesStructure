using System.Web.Mvc;

namespace EmployeesStructure.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Employee structure";

            return View();
        }
    }
}
