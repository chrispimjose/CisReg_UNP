using MongoDB.Bson;

namespace CisReg_Website.Models.Vacancy;

public class VacancyAssignCreateViewModel
{
  public ObjectId ProfessionalId { get; set; }
  public ObjectId PatientId { get; set; }
}