using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisReg_Website.Models;
using MongoDB.Bson;
using CisReg_Website.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;

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
                    HallSpecialties = hall.specialties,
                    HallId    =      hall.Id
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
                        HallSpecialties = hall.specialties,
                         HallId = hall.Id.ToString()
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
            var vacancies = await _context.Vacancies
            .Where(v => v.Id == id)  // Filtra vagas associadas ao Hall
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
                    SusC = patient?.SusCard,
                    Mother = patient?.MotherName,
                    Father= patient?.FatherName,
                    cnes= patient?.Cnes,
                    php= patient?.Phone,
                    PatientId= patient?.Id,
                   
                };

                // Adiciona os dados da vaga à lista
                vacancyDetails.Add(vacancyData);
            }

          
            // Obtenha todas as formações acadêmicas dos profissionais
            var academicDetails = _context.Professionals
                .Select(p => p.Academic)
                .Distinct()
                .ToList();

            // Passe os detalhes acadêmicos para a View
            ViewData["academicDetails"] = academicDetails;
            ViewData["VacancyDetails"] = vacancyDetails;
            return View();
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
        [HttpPost]
        public IActionResult Preenchimento(string id)
        {
          

            // Passa o CNPJ para a view
            ViewData["id"] = id;
            // Obtenha todas as formações acadêmicas dos profissionais
            var academicDetails = _context.Professionals
                .Select(p => p.Academic)
                .Distinct()
                .ToList();

            // Passe os detalhes acadêmicos para a View
            ViewData["academicDetails"] = academicDetails;

            return View();
        }

        [HttpGet]
        public IActionResult GetEspecialidadesPorFormacao(string formacaoAcademica)
        {
            if (string.IsNullOrWhiteSpace(formacaoAcademica))
            {
                return Json(new { error = "Formação acadêmica inválida." });
            }

            try
            {
                // Busca os profissionais com a formação acadêmica específica e retorna suas especialidades
                var especialidades = _context.Professionals
                    .Where(p => p.Academic == formacaoAcademica)  // Filtra pela formação acadêmica
                    .Select(p => p.Specialty)  // Seleciona a especialidade de cada profissional
                    .Distinct()  // Remove duplicados, se houver
                    .ToList();  // Executa a consulta e retorna a lista

                // Se não houver especialidades encontradas
                if (!especialidades.Any())
                {
                    return Json(new { error = "Nenhuma especialidade encontrada." });
                }

                // Retorna as especialidades para o front-end
                return Json(new { especialidades = especialidades.Select(e => new { nome = e }).ToList() });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Erro ao buscar especialidades: " + ex.Message +   formacaoAcademica});
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateVacancy(string id, string firstName, string lastName, string cpf, DateTime dob, DateTime date, string susCard,
                                                string cid, string phone, string motherName, string fatherName, string academic,
                                                string specialty,DateTime SelectedDateTime)
        {
            // Verifica se os campos obrigatórios foram preenchidos
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(cpf))
            {
                ViewData["ErrorMessage"] = "Preencha todos os campos obrigatórios!";
                return View(); // Retorna a view de criação com erro
            }
 DateTime selectedDateTime = SelectedDateTime;
            var patient = new Patient();

            // Atribuindo os valores diretamente
            patient.FirstName = firstName;
            patient.LastName = lastName;
            patient.Email = $"{firstName.ToLower()}.{lastName.ToLower()}@teste.com"; // Gera um email fictício
            patient.Password = "senhaTeste"; // Em produção, use hash de senha
            patient.Cnes = cid;
            patient.BirthDate = dob;
            patient.SusCard = susCard;
            patient.Phone = phone;
            patient.FatherName = fatherName;
            patient.MotherName = motherName;

            // Adiciona o paciente ao contexto e salva
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();


            // 2. Criar o Profissional (Professional)
            var professional = await _context.Professionals
                .FirstOrDefaultAsync(p => p.Academic == academic && p.Specialty == specialty);

            if (professional == null)
            {
                ViewData["ErrorMessage"] = "Profissional com a formação acadêmica e especialidade fornecidas não encontrado!";
                return View(); // Retorna a view de criação com erro
            }
           

            // Adicionar no ViewData com formato adequado
            ViewData["ErrorMessage"] = selectedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            var vacancy = new VacancyModel
            {
                AvailableHour = selectedDateTime, // Exemplo, isso pode vir de algum campo de formulário ou lógica
                Status = Status.Available, // Inicializando com o status "Vago"
                PatientId = patient.Id, // Atribuindo o ID do paciente criado
                ProfessionalId = professional.Id, // Atribuindo o ID do profissional encontrado
                ReservedById = new ObjectId(id), // ID do Hall (salvo no formulário ou no banco de dados)
                CreatedById = new ObjectId(), // Este valor precisa ser obtido com base no usuário autenticado
            };

            // Adiciona a vaga ao contexto e salva
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            // Redireciona para a página de detalhes ou a página inicial
            return RedirectToAction(nameof(Index)); // Ou outra ação desejada após criação
        }

        public async Task<IActionResult> EditVacancy(string id,string patientId, string firstName, string lastName, DateTime dob, DateTime data, string susCard,
                                                    string cid, string phone, string motherName, string fatherName, string academic, string specialty)
        {
     
            // Buscar o paciente pelo ID
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id.ToString() == patientId);


            // Atualizar as informações do paciente
            patient.FirstName = firstName;
            patient.LastName = lastName;
            patient.Email = $"{firstName.ToLower()}.{lastName.ToLower()}@teste.com"; // Gera um email fictício
            patient.Cnes = cid;
            patient.BirthDate = dob;
            patient.SusCard = susCard;
            patient.Phone = phone;
            patient.FatherName = fatherName;
            patient.MotherName = motherName;

            // Salva as atualizações do paciente
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();

            // Buscar o profissional com a formação acadêmica e especialidade fornecidas
            var professional = await _context.Professionals
                .FirstOrDefaultAsync(p => p.Academic == academic && p.Specialty == specialty);

            if (professional == null)
            {
                ViewData["ErrorMessage"] = "Profissional com a formação acadêmica e especialidade fornecidas não encontrado!";
                return View(); // Retorna a view de edição com erro
            }

            // Buscar todas as vagas associadas ao PatientId
            var vacancies = await _context.Vacancies
                .Where(v => v.PatientId == patient.Id)  // Filtra vagas associadas ao paciente
                .ToListAsync();

            var vacancyDetails = new List<object>(); // Lista para armazenar os detalhes das vagas

            foreach (var vacancy in vacancies)
            {
                // Para cada vaga, buscar o paciente e o profissional associados
                var vacancyPatient = vacancy.PatientId.HasValue ? await _context.Patients.FindAsync(new ObjectId(vacancy.PatientId.Value.ToString())) : null;
                var vacancyProfessional = vacancy.ProfessionalId.HasValue ? await _context.Professionals.FindAsync(new ObjectId(vacancy.ProfessionalId.Value.ToString())) : null;

                // Adiciona os detalhes da vaga à lista
                vacancyDetails.Add(new
                {
                    Vacancy = vacancy,
                    Patient = vacancyPatient,
                    Professional = vacancyProfessional
                });
            }

            // Se não encontrar nenhuma vaga associada, retorne um erro
            if (vacancyDetails.Count == 0)
            {
                ViewData["ErrorMessage"] = "Nenhuma vaga associada ao paciente foi encontrada!";
                return View();
            }

            // Atualizar os dados da vaga para o paciente (caso haja uma vaga para atualizar)
            var vacancyToUpdate = vacancies.FirstOrDefault(); // Exemplo: seleciona a primeira vaga associada ao paciente
            if (vacancyToUpdate != null)
            {
                vacancyToUpdate.AvailableHour = data;
                vacancyToUpdate.Status = Status.Available; // Ou outro status desejado
                vacancyToUpdate.ProfessionalId = professional.Id;

                // Salvar as atualizações da vaga
                _context.Vacancies.Update(vacancyToUpdate);
                await _context.SaveChangesAsync();
            }

            // Redireciona para a página de detalhes ou a página inicial
            return RedirectToAction(nameof(Index)); // Ou outra ação desejada após edição
        }

        public async Task<IActionResult> GetProfissionaisPorEspecialidadeEFormacao(string especialidade, string formacaoAcademica)
        {
            var profissionais = await _context.Professionals
                .Where(p => p.Specialty == especialidade && p.Academic == formacaoAcademica)
                .ToListAsync();

            if (profissionais == null || !profissionais.Any())
            {
                return Json(new { profissionais = new List<object>() });
            }

            // Ajustar para incluir ID, FirstName e LastName
            var result = profissionais.Select(p => new
            {
                id = p.Id, // Adiciona o ID do profissional
                firstName = p.FirstName,
                lastName = p.LastName
            }).ToList();

            return Json(new { profissionais = result });
        }



    }



}
