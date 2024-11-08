using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Controllers {
    public class HomeController1 : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}