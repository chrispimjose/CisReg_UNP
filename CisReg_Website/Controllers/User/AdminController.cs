using CisReg_Website.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Controllers.User
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
