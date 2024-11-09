using CisReg_Website.Domain;
using CisReg_Website.Models.Vacancy;
using MongoDB.Bson;

namespace CisReg_Website.Repositories;

public class VacancyRepository(ApplicationDbContext context)
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<VacancyModel> GetAll()
  {
    return [.. _context.Vacancies];
  }

  public VacancyModel? GetById(ObjectId Id)
  {
    return _context.Vacancies.FirstOrDefault(v => v.Id == Id);
  }
}