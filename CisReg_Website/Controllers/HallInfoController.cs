using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using MongoDB.Bson;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;

namespace CisReg_Website.Controllers
{
    public class HallInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HallInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Obtém o JSON armazenado no TempData para as informações do Hall
            var modelJson = TempData["PersonalInfo"] as string;

            // Deserializa ou cria um novo objeto Hall
            var hall = string.IsNullOrEmpty(modelJson) 
                ? new Hall() 
                : JsonConvert.DeserializeObject<Hall>(modelJson);

            // Garante que o objeto "combinedModel" receba as propriedades corretamente
            var combinedModel = new Hall
            {
                CNPJ = hall.CNPJ,
                Cnes = hall.Cnes,
                Address = hall.Address,
                CityHallName = hall.CityHallName,
                AgreementNumber = hall.AgreementNumber,
                CityHallManager = hall.CityHallManager,
                ResponsiblePhoneNumber = hall.ResponsiblePhoneNumber,
                ResponsibleEmail = hall.ResponsibleEmail
            };

            return View("~/Views/Hall/Layout.cshtml", combinedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Hall model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Erro no envio. Verifique os dados e tente novamente.";
                return View(model);
            }

            try
            {
                // Obtém informações armazenadas no TempData
                var modelJson = TempData["CombinedInfo"] as string;

                // Configurações para lidar com o tipo ObjectId
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new ObjectIdConverter());

                // Deserializa o modelo combinado ou usa o modelo enviado
                var combinedModel = string.IsNullOrEmpty(modelJson)
                    ? model
                    : JsonConvert.DeserializeObject<Hall>(modelJson, settings);

                // Adiciona o modelo ao contexto do banco de dados
                _context.Add(combinedModel);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Login");
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorMessage = "Erro ao salvar os dados no banco. Tente novamente.";
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro inesperado. Tente novamente.";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }

    public class ObjectIdConverter : JsonConverter<ObjectId>
    {
        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Lê o valor do JSON e converte para ObjectId
            var objectIdString = reader.Value as string;
            return string.IsNullOrEmpty(objectIdString) ? ObjectId.Empty : new ObjectId(objectIdString);
        }

        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            // Escreve o valor do ObjectId como string no JSON
            writer.WriteValue(value.ToString());
        }
    }
}
