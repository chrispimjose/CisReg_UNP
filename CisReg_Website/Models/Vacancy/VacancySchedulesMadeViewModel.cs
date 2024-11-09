using MongoDB.Bson;

namespace CisReg_Website.Models.Vacancy;

public enum Period
{
  Morning,
  Afternoon,
  Night
}

public class VacancyComplete
{
  public ObjectId Id { get; set; }

  public DateTime AvailableHour { get; set; }

  public Patient? Patient { get; set; }

  public Professional? Professional { get; set; }

  public UserModel? ReservedBy { get; set; }

  public UserModel? CreatedBy { get; set; }

  public Status Status { get; set; }
}

public class VacancySchedulesMadeQueryParams
{
  public Period? Period { get; set; }
  public string? ProfessionalSpecialty { get; set; }
  public Status[]? Status { get; set; }
  public DateTime? InitialDate { get; set; }
  public DateTime? FinalDate { get; set; }
}

public class VacancySchedulesMadeViewModel(IEnumerable<VacancyComplete> vacancies)
{
  public IEnumerable<VacancyComplete> Vacancies { get; set; } = vacancies;
}