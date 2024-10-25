using Microsoft.AspNetCore.Mvc;
using CisReg_Website.Models;

namespace CisReg_Website.Controllers
{
    public class AgendamentoController : Controller
    {
        // Simulação de banco de dados
        private static List<AgendamentoModel> agendamentos = new List<AgendamentoModel>();

            // Ação para exibir o formulário de agendamento
        public IActionResult PreenchimentoVagas()
        {
            return View(); // Certifique-se de que a view se chama "PreenchimentoVagas.cshtml"
        }

        [HttpPost]
        public IActionResult Agendar(PacienteModel paciente, string unidadeAtendimento, string especialidade, DateOnly data, TimeOnly horario)
        {
            // Validação básica
            if (!ModelState.IsValid)
            {
                return View("Erro", ModelState);
            }

            // Criar um novo agendamento
            var novoAgendamento = new AgendamentoModel
            {
                NomePaciente = paciente.NomePaciente,
                Cpf = paciente.Cpf,
                UnidadeAtendimento = unidadeAtendimento,
                Especialidade = especialidade,
                DataAgendamento = data,
                HorarioAgendamento = horario
            };

            // Adicionar o agendamento à lista (simulação de banco)
            agendamentos.Add(novoAgendamento);

            // Mensagem de sucesso
            TempData["MensagemSucesso"] = "Agendamento realizado com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
