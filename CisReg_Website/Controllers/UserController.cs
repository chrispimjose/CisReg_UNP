using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Domain;
using MongoDB.Bson;

namespace CisReg_Website.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult SchedulesMade()
        {
            return View();
        }

        private bool UserModelExists(ObjectId id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
