using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using MongoDB.Bson;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using CisReg_Website.Repositories;

namespace CisReg_Website.Controllers
{
    public class ProfessionalInfoController(ApplicationDbContext context, ProfessionalRepository professionalRepository) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ProfessionalRepository _professionalRepository = professionalRepository;

        public class ObjectIdConverter : JsonConverter<ObjectId>
        {
            public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var objectIdString = reader.Value as string;
                return string.IsNullOrEmpty(objectIdString) ? ObjectId.Empty : new ObjectId(objectIdString);
            }

            public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }

        [HttpGet]
        public IActionResult Index()
        {

            var formationsList = _professionalRepository.GetAllFormations();
            var specialtiesList = _professionalRepository.GetAllSpecialties();

            ViewBag.Formations = formationsList;
            ViewBag.Specialties = specialtiesList;

            return View("~/Views/Registration/ProfessionalInfo.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Professional model)
        {
            var personalInfoJson = TempData["PersonalInfo"] as string;

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            var professionalModel = string.IsNullOrEmpty(personalInfoJson)
                ? new Professional()
                : JsonConvert.DeserializeObject<Professional>(personalInfoJson, settings);

            professionalModel.Council = model.Council;
            professionalModel.Academic = model.Academic;
            professionalModel.Specialty = model.Specialty;
            professionalModel.Permission = Permissions.Professional;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio dos dados";
                return View(model);
            }
            try
            {
                TempData["ProfessionalInfo"] = JsonConvert.SerializeObject(professionalModel);

                _context.Add(professionalModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Login");
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorMessage = "Erro no envio dos dados...";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Erro inesperado. Tente novamente.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    }
}
