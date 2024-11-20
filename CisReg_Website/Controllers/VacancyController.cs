using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisReg_Website.Models;
using MongoDB.Bson;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;

namespace CisReg_Website.Controllers
{
    public class VacancyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacancyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string id = "6737780d104e9aafd1f34972")
        {
            // Obtém o HallModel usando o ID fornecido (assumindo que 'id' é o ObjectId do Hall)
            var hall = await _context.Halls.FindAsync(new ObjectId(id));

            // Se o Hall não for encontrado, retornamos um erro ou alguma lógica alternativa
            if (hall == null)
            {
                return NotFound($"Hall com o ID {id} não encontrado.");
            }

            // Obtém a lista de vagas associadas a este Hall
            var vacancies = await _context.Vacancies
                .Where(v => v.ReservedById == hall.Id)  // Filtra vagas associadas ao Hall
                .ToListAsync();

            var vacancyDetails = new List<object>(); // Lista para guardar dados de vagas com pacientes, profissionais e halls

            foreach (var vacancy in vacancies)
            {
                var patient = vacancy.PatientId.HasValue ? await _context.Patients.FindAsync(new ObjectId(vacancy.PatientId.Value.ToString())) : null;
                var professional = vacancy.ProfessionalId.HasValue ? await _context.Professionals.FindAsync(new ObjectId(vacancy.ProfessionalId.Value.ToString())) : null;

                // Prepara os dados da vaga
                var vacancyData = new
                {
                    VacancyId = vacancy.Id,
                    AvailableHour = vacancy.AvailableHour,
                    Status = vacancy.Status.ToString(),
                    // Dados do paciente
                    PatientName = patient?.FatherName,
                    PatientFirstName = patient?.FirstName,
                    PatientLastName = patient?.LastName,
                    PatientEmail = patient?.Email,
                    // Dados do profissional
                    ProfessionalName = professional?.FirstName + " " + professional?.LastName,
                    ProfessionalEmail = professional?.Email,
                    ProfessionalSpecialty = professional?.Specialty,
                    ProfessionalAcademic = professional?.Academic,
                    // Dados do Hall (agreement e specialties)
                    HallAgreement = hall.Agreement,
                    HallSpecialties = hall.specialties
                };

                // Adiciona os dados da vaga à lista
                vacancyDetails.Add(vacancyData);
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
                        
                    });
                }

                // Adicionar os cards vazios à lista de vagas
                foreach (var emptyVacancy in emptyVacancies)
                {
                    var emptyVacancyData = new
                    {
                        VacancyId = emptyVacancy.Id,
                        AvailableHour = "N/A", // Para os cards vazios, a hora pode ser 'N/A'
                        Status = "Vazio",
                        HallAgreement = hall.Agreement,
                        HallSpecialties = hall.specialties
                    };

                    vacancyDetails.Add(emptyVacancyData);
                }
            }

            // Passa os detalhes para a view
            ViewData["VacancyDetails"] = vacancyDetails;

            return View(vacancies);
        }


        public async Task<IActionResult> Index2()
        {
            // Obtém a lista de vagas
            var vacancies = await _context.Vacancies.ToListAsync();

            var patientDetails = new List<object>(); // Lista para guardar dados dos pacientes
            var professionalDetails = new List<object>(); // Lista para guardar dados dos profissionais

            foreach (var vacancy in vacancies)
            {
                // Processa dados do paciente
                if (vacancy.PatientId.HasValue)
                {
                    var patientId = new ObjectId(vacancy.PatientId.Value.ToString());
                    var patient = await _context.Patients.FindAsync(patientId);

                    if (patient != null)
                    {
                        patientDetails.Add(new
                        {
                            VacancyId = vacancy.Id,
                            AvailableHour = vacancy.AvailableHour,
                            Status = vacancy.Status.ToString(),
                            PatientName = patient.FatherName,
                            PatientEmail = patient.Email,
                            PatientFirstName = patient.FirstName,
                            PatientLastName = patient.LastName,
                            PatientCNES = patient.Cnes,
                            PatientBirthDate = patient.BirthDate,
                            PatientSusCard = patient.SusCard,
                            PatientPhone = patient.Phone,
                            PatientFatherName = patient.FatherName,
                            PatientMotherName = patient.MotherName
                        });
                    }
                }

                // Processa dados do profissional
                if (vacancy.ProfessionalId.HasValue)
                {
                    var professionalId = new ObjectId(vacancy.ProfessionalId.Value.ToString());
                    var professional = await _context.Professionals.FindAsync(professionalId);

                    if (professional != null)
                    {
                        professionalDetails.Add(new
                        {
                            VacancyId = vacancy.Id,
                            AvailableHour = vacancy.AvailableHour,
                            Status = vacancy.Status.ToString(),
                            ProfessionalEmail = professional.Email,
                            ProfessionalFirstName = professional.FirstName,
                            ProfessionalLastName = professional.LastName,
                            ProfessionalAcademic = professional.Academic,
                            ProfessionalCouncil = professional.Council,
                            ProfessionalCouncilNumber = professional.CouncilNumber,
                            ProfessionalSpecialty = professional.Specialty
                        });
                    }
                }
            }

            // Passa os detalhes para a view (opcional)
            ViewData["PatientDetails"] = patientDetails;
            ViewData["ProfessionalDetails"] = professionalDetails;

            return View(vacancies);
        }




        public async Task<IActionResult> Details(ObjectId id)
        {
            var vacancyModel = await _context.Vacancies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancyModel == null)
            {
                return NotFound();
            }

            return View(vacancyModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AvailableHour,PatientId,ProfessionalId,ReservedById,CreatedById,Status")] VacancyModel vacancyModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacancyModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacancyModel);
        }

        public async Task<IActionResult> Edit(ObjectId id)
        {
            var vacancyModel = await _context.Vacancies.FindAsync(id);
            if (vacancyModel == null)
            {
                return NotFound();
            }
            return View(vacancyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ObjectId id, [Bind("Id,AvailableHour,PatientId,ProfessionalId,ReservedById,CreatedById,Status")] VacancyModel vacancyModel)
        {
            if (id != vacancyModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancyModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyModelExists(vacancyModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacancyModel);
        }

        public async Task<IActionResult> Delete(ObjectId id)
        {
            var vacancyModel = await _context.Vacancies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancyModel == null)
            {
                return NotFound();
            }

            return View(vacancyModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ObjectId id)
        {
            var vacancyModel = await _context.Vacancies.FindAsync(id);
            if (vacancyModel != null)
            {
                _context.Vacancies.Remove(vacancyModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacancyModelExists(ObjectId id)
        {
            return _context.Vacancies.Any(e => e.Id == id);
        }
        public IActionResult Preenchimento(string cnpj= "6737780d104e9aafd1f34972")
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

    }
}