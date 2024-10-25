using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;
using Newtonsoft.Json;
using CisReg_Website.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CisReg_Website.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(LoginModel model)
        {    
            if (ModelState.IsValid)
            {
                var filtro = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("Email", model.Email),
                    Builders<BsonDocument>.Filter.Eq("Password", model.Password)
                    );
                var result = Database.GetInstance().Select("profissional", filtro);

                if (result != null && result.Count > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
    }
}
