using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using CisReg_Website.Repositories;
using MongoDB.Bson;
using System.Drawing.Drawing2D;

namespace CisReg_Website.Controllers
{
    public class PersonalInfoController(ApplicationDbContext context, ProfessionalRepository professionalRepository) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ProfessionalRepository _professionalRepository = professionalRepository;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Submit(Professional model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio...";
                return View("~/Views/Registration/PersonalInfo.cshtml", model);
            }
            try
            {
                var searchForCPF = _professionalRepository.SearchForProfessionalCPF(model);

                if (searchForCPF != null)
                {
                    ViewBag.ErrorMessage = "O CPF informado já está cadastrado.";
                    return View("~/Views/Registration/PersonalInfo.cshtml", model);
                }

                if (model.Id == ObjectId.Empty)
                {
                    model.Id = ObjectId.GenerateNewId();
                }

                TempData["PersonalInfo"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("Index", "ProfessionalInfo");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorMessage = "Erro no envio dos dados...";

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro inesperado. Tente novamente.";
            }

            return View("~/Views/Registration/PersonalInfo.cshtml", model);
        }

        [HttpGet]
        public IActionResult ProfessionalInfo()
        {
            return View();
        }
    }
}
