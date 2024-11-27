namespace CisReg_Website.Models.ProfessionalModels;
using CisReg_Website.Models;
using CisReg_Website.Models.Vacancy;

public class SelectProfessionalQueryParams
{
    public Period? Period { get; set; }
    public IEnumerable<string>? ProfessionalSpecialties { get; set; }
    public DateTime? Date { get; set; }
    public string? Search { get; set; }
}

public class SelectProfessionalViewModel(
  IEnumerable<Professional> professionals,
  IEnumerable<string> professionalSpecialties,
  SelectProfessionalQueryParams queryParams
)
{
    public IEnumerable<Professional> Professionals { get; set; } = professionals;
    public IEnumerable<string> ProfessionalSpecialties { get; set; } = professionalSpecialties;
    public SelectProfessionalQueryParams QueryParams { get; set; } = queryParams;
}