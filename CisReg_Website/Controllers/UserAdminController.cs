using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Controllers
{
    public class UserAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
