using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisReg_Website.Models;
using MongoDB.Bson;

namespace CisReg_Website.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IMongoCollection<VacancyModel> _vacancies;
        private readonly IMongoCollection<HallModel> _halls;

        public VacancyController(IMongoDatabase database)
        {
            _vacancies = database.GetCollection<VacancyModel>("Vacancys");
            _halls = database.GetCollection<HallModel>("Halls");
        }

        // Método para listar vagas associadas a um CNPJ específico
        


    }
}