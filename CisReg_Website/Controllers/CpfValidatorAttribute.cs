using System.ComponentModel.DataAnnotations;
using System.Linq;

public class CpfValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        // Verifica se o valor é nulo
        if (value is null)
        {
            return new ValidationResult("CPF é obrigatório.");
        }

        // Tenta converter o value para string de forma segura
        if (value is string cpf)
        {
            // Valida o CPF
            if (string.IsNullOrEmpty(cpf) || !IsCpfValid(cpf))
            {
                return new ValidationResult("CPF inválido.");
            }
        }
        else
        {
            // Retorna erro se o tipo não for string
            return new ValidationResult("Tipo de CPF inválido.");
        }

        return ValidationResult.Success ?? new ValidationResult(null);
    }

    private bool IsCpfValid(string cpf)
    {
        // Remove caracteres especiais (pontos e traços)
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        // Verifica o comprimento e se todos os dígitos são iguais
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        // Algoritmo de validação do CPF
        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        // Primeiro dígito verificador
        string tempCpf = cpf.Substring(0, 9);
        int soma = tempCpf.Select((t, i) => (int)char.GetNumericValue(t) * multiplicador1[i]).Sum();
        int resto = (soma * 10) % 11;

        if (resto == 10 || resto == 11)
            resto = 0;

        if (resto != (int)char.GetNumericValue(cpf[9]))
            return false;

        // Segundo dígito verificador
        tempCpf += cpf[9];
        soma = tempCpf.Select((t, i) => (int)char.GetNumericValue(t) * multiplicador2[i]).Sum();
        resto = (soma * 10) % 11;

        if (resto == 10 || resto == 11)
            resto = 0;

        return resto == (int)char.GetNumericValue(cpf[10]);
    }
}
