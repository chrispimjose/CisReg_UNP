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

            var filter = Builders<VacancyModel>.Filter.Eq("created_by.hall.cnpj", cnpj);
            var vacancies = await _vacancies.Find(filter).ToListAsync();

            // Obter o valor de Agreement do Hall
            var hallFilter = Builders<HallModel>.Filter.Eq(h => h.CNPJ, cnpj);
            var hall = await _halls.Find(hallFilter).FirstOrDefaultAsync();

            if (hall == null)
            {
                return NotFound("Hall não encontrado.");
            }

            // Calcular a quantidade de cards vazios necessários
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
                        CreatedBy = new UserHall
                        {
                            Hall = new HallModel
                            {
                                CNPJ = cnpj // Atribuindo o CNPJ aos cards vazios
                            }
                        }
                    });
                }

                // Adicionar os cards vazios à lista de vagas
                vacancies.AddRange(emptyVacancies);
            }

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
        public IActionResult Preenchimento(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return BadRequest("CNPJ não fornecido.");
            }

            // Passa o CNPJ para a view
            ViewData["CNPJ"] = cnpj;

            // Outros dados necessários para o formulário de preenchimento
            return View();
        }


        // Processa o preenchimento e cria a vaga
        [HttpPost]
        public async Task<IActionResult> Preenchimento(VacancyModel model, string cnpj)
        {
            if (ModelState.IsValid)
            {
                // Criar uma nova vaga com os dados do formulário
                var newVacancy = new VacancyModel
                {
                    // Garantir que Patient não seja null
                    Patient = new Patient
                    {
                        FirstName = model.Patient.FirstName,  // Usar o operador de condicional para evitar problemas se Patient for null
                        LastName = model.Patient?.LastName,
                        Cnes = model.Patient?.Cnes,
                        BirthDate = model.Patient?.BirthDate,
                        SusCard = model.Patient?.SusCard,
                        Phone = model.Patient?.Phone,
                        FatherName = model.Patient?.FatherName,
                        MotherName = model.Patient?.MotherName
                    },
                    Professional = new Professional
                    {
                        FirstName = model.Professional?.FirstName,
                        LastName = model.Professional?.LastName,
                        Academic = model.Professional?.Academic,
                        Council = model.Professional?.Council,
                        CouncilNumber = model.Professional?.CouncilNumber
                    },
                    AvailableHour = model.AvailableHour,
                    Status = Status.Occupied, // Definir o status como "Ocupado"
                    CreatedBy = new UserHall
                    {
                        Hall = new HallModel
                        {
                            CNPJ = cnpj,
                           specialties = model.CreatedBy.Hall.specialties
                        }
                    }
                };

                // Inserir a nova vaga no banco de dados
                await _vacancies.InsertOneAsync(newVacancy);

                // Redireciona para a página de gerenciamento de vagas, passando o CNPJ
                return RedirectToAction("Index", new { cnpj });
            }

            // Se o formulário não for válido, retorna a mesma view para correção
            return View(model);
        }




    }
}
