using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisReg_Website.Models
{
    public class PacienteModel
    {
        public int IdPaciente { get; set; }

        public required string NomePaciente { get; set; }

        public DateTime DataNascimento { get; set; }

        [CpfValidator]
        public required string Cpf { get; set; }

        public required string Telefone { get; set; }

        public required int CartaoSus { get; set; }

        public string? NomePai { get; set; }

        public required string NomeMae { get; set; }

        public int Cnes { get; set; }
        
    }
}