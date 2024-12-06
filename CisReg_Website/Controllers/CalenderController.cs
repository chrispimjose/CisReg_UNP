using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Controllers
{
    public class CalenderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
