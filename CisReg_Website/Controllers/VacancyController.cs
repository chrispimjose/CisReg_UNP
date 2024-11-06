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
        public async Task<IActionResult> Index(string cnpj = "98765432000198")
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return BadRequest("CNPJ não fornecido.");
            }

            // Define o filtro para buscar todas as vagas com o CNPJ específico dentro de `created_by.hall.cnpj`
            var filter = Builders<VacancyModel>.Filter.Eq("created_by.hall.cnpj", cnpj);

            // Executa a busca
            var vacancies = await _vacancies.Find(filter).ToListAsync();

            if (vacancies == null || vacancies.Count == 0)
            {
                return NotFound("Nenhuma vaga encontrada para o CNPJ fornecido.");
            }

            // Passa as vagas para a View
            return View(vacancies);
        }

        // Método para obter detalhes de uma vaga específica (usando ID)
        public async Task<IActionResult> Details(string vacancyId)
        {
            if (!ObjectId.TryParse(vacancyId, out ObjectId objectId))
            {
                return BadRequest("ID de vaga inválido.");
            }

            // Busca a vaga pelo ID convertido
            var vacancy = await _vacancies.Find(v => v.Id == objectId).FirstOrDefaultAsync();
            if (vacancy == null)
            {
                return NotFound();
            }

            // Passa a vaga para a View
            return View(vacancy);
        }
    }
}
