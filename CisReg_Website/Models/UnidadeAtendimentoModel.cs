using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisReg_Website.Models
{
    public class UnidadeAtendimentoModel
    {
        public int IdUnidadeAtendimento { get; set; }

        public required string NomeUnidade { get; set; }

        public required string Endereco { get; set; }

        public required string Telefone { get; set; }

        public int IdEspecialidade { get; set; }

        public required string DescricaoEspecialidade { get; set; }

    }
}