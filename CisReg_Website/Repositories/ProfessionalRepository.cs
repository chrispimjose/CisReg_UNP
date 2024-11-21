using CisReg_Website.Domain;
using CisReg_Website.Models;
using CisReg_Website.Models.ProfessionalModels;

namespace CisReg_Website.Repositories;

public class ProfessionalRepository(ApplicationDbContext context)
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<Professional> GetAll()
  {
    return [.. _context.Professionals.Where(u => u.Permission == Permissions.Professional)];
  }

  public IEnumerable<string> GetAllSpecialties()
  {
    return [.. _context.Professionals
    .Where(u => u.Permission == Permissions.Professional)
    .Select(u => u.Specialty)
    .Where(s => !string.IsNullOrEmpty(s))
    .Distinct()
    .Select(s => s!)];
  }

  public IEnumerable<Professional> GetAllByQuery(SelectProfessionalQueryParams Params)
  {
    Console.WriteLine(Params.ProfessionalSpecialties?.ToString());

    var professionals = _context.Professionals
      .Where(u => u.Permission == Permissions.Professional)
      .ToList()
      .Where(u => string.IsNullOrEmpty(Params.Search) || IsSearchInUsersInfo(u, Params.Search))
      .Where(u => string.IsNullOrEmpty(Params.ProfessionalSpecialties?.ToString()) ||
        (Params.ProfessionalSpecialties?.Contains(u.Specialty) ?? false));

    return professionals;
  }

  private static bool IsSearchInUsersInfo(Professional professional, string search)
  {
    return (professional?.FirstName + " "
      + professional?.LastName + " "
      + professional?.Council + " "
      + professional?.CouncilNumber).Contains(search);
  }
}