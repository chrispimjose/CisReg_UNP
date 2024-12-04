using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Data
{
    public class MongoDbContext : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
