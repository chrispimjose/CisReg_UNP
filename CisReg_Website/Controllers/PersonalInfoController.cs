using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;

namespace CisReg_Website.Controllers
{
    public class PersonalInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Submit(PersonalInfoModel model)
        {

            if (ModelState.IsValid)
            {
                var searchForCPF = await _context.Professional
                    .FirstOrDefaultAsync(m => m.CPF == model.CPF);

                if (searchForCPF == null)
                {
                    TempData["PersonalInfo"] = JsonConvert.SerializeObject(model);
                    return RedirectToAction("Index", "ProfessionalInfo");
                }
                else
                {
                    ViewBag.ErrorMessage = "O CPF informado já está cadastrado.";
                    return View("~/Views/Registration/PersonalInfo.cshtml", model);
                }
            }

            ViewBag.ErrorMessage = "Invalid...";
            return View("~/Views/Registration/PersonalInfo.cshtml", model);
        }

        [HttpGet]
        public IActionResult ProfessionalInfo()
        {
            return View();
        }
    }
}
