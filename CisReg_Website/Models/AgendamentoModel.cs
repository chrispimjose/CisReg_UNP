using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisReg_Website.Models
{
    public class AgendamentoModel
    {
        public int IdAgendamento { get; set; }
        
        public int IdPaciente { get; set; }

        public required string NomePaciente { get; set; }
        
        public required string Cpf { get; set; }

        public required string Especialidade { get; set; }

        public required string UnidadeAtendimento { get; set; }

        public int IdVaga { get; set; }

        public DateOnly DataAgendamento { get; set; }

        public TimeOnly HorarioAgendamento { get; set; }

    }
}