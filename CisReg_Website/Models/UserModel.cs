using System.ComponentModel.DataAnnotations.Schema;
using CisReg_Website.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;

public interface IVacancyReserver { }
public interface IVacancyCreator { }

public enum Permissions
{
  Admin,
  SupUnp,
  SupHall,
  Patient,
  UserHall,
  Professional
}

public class UserModel
{
  [BsonId]
  public ObjectId Id { get; set; }

  [BsonElement("email")]
  public string? Email { get; set; }

  [BsonElement("password")]
  public string? Password { get; set; }

  [BsonElement("first_name")]
  public string? FirstName { get; set; }

  [BsonElement("last_name")]
  public string? LastName { get; set; }

  [BsonElement("permission")]
  [BsonRepresentation(BsonType.String)]
  public Permissions? Permission { get; set; }
}

public class Patient : UserModel
{
  private readonly ApplicationDbContext _context;

  [BsonElement("cnes")]
  public string? Cnes { get; set; }

  [BsonElement("birth_date")]
  public DateTime? BirthDate { get; set; }

  [BsonElement("sus_card")]
  public string? SusCard { get; set; }

  [BsonElement("phone")]
  public string? Phone { get; set; }

  [BsonElement("father_name")]
  public string? FatherName { get; set; }

  [BsonElement("mother_name")]
  public string? MotherName { get; set; }

  public Patient(ApplicationDbContext context)
  {
    _context = context;
  }

  public IEnumerable<Patient> GetAll()
  {
    return [.. _context.Patients.Where(u => u.Permission == Permissions.Patient)];
  }
}

public class Professional(ApplicationDbContext context) : UserModel
{
  private readonly ApplicationDbContext _context = context;

  [BsonElement("academic")]
  public string? Academic { get; set; }

  [BsonElement("council")]
  public string? Council { get; set; }

  [BsonElement("council_number")]
  public string? CouncilNumber { get; set; }

  public string? Specialty { get; set; }
  public string? Formation { get; set; }

  public IEnumerable<Professional> GetAll()
  {
    return [.. _context.Professionals.ToList()];
  }
}

public class UserHall : UserModel
{
  [BsonElement("hall")]
  public string? HallModel { get; set; }
}

public class SupUnp : UserModel, IVacancyReserver, IVacancyCreator
{
}

public class SupHall : UserHall, IVacancyCreator
{
}

public class Admin : UserModel, IVacancyReserver, IVacancyCreator
{
}