using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using CisReg_Website.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CisReg_Website.Controllers
{
    public class LoginController : Controller
    {

        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var professionalTable = await _context.Professional
                     .FirstOrDefaultAsync(m => m.Email == model.Email);

                if (professionalTable == null)
                {
                    return View("Index");
                }

                if (model.Password != professionalTable.Password)
                {
                    return View("Index");
                }


                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
    }
}
