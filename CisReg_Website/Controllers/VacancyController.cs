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
            

            var filter = Builders<VacancyModel>.Filter.Eq("created_by.hall.cnpj", cnpj);
            var vacancies = await _vacancies.Find(filter).ToListAsync();

            

            // Obter o valor de Agreement do Hall
            var hallFilter = Builders<HallModel>.Filter.Eq(h => h.CNPJ, cnpj);
            var hall = await _halls.Find(hallFilter).FirstOrDefaultAsync();

            if (hall == null)
            {
                return NotFound("Hall não encontrado.");
            }

            // Calcular a quantidade de cards vazios
            int emptyCardsNeeded = hall.Agreement - vacancies.Count;
            if (emptyCardsNeeded > 0)
            {
                // Criar cards vazios
                var emptyVacancies = new List<VacancyModel>();
                for (int i = 0; i < emptyCardsNeeded; i++)
                {
                    emptyVacancies.Add(new VacancyModel
                    {
                        Status = Status.Vazio, // Marcar como Vazio
                    });
                }

                // Adicionar os cards vazios à lista de vagas
                vacancies.AddRange(emptyVacancies);
            }

            return View(vacancies);
        }

        public IActionResult Preenchimento()
        {
            
            // Retorna a vaga para ser preenchida
            return View();
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
