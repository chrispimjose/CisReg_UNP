using System.ComponentModel;
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
  [DisplayName("Email")]
  public string? Email { get; set; }

  [BsonElement("password")]
  [DisplayName("Senha")]
  public string? Password { get; set; }

  [BsonElement("first_name")]
  [DisplayName("Nome")]
  public string? FirstName { get; set; }

  [BsonElement("last_name")]
  [DisplayName("Sobrenome")]
  public string? LastName { get; set; }

  [BsonElement("permission")]
  [DisplayName("Permissão")]
  [BsonRepresentation(BsonType.String)]
  public Permissions? Permission { get; set; }

  public UserModel()
  {

  }
}

public class Patient : UserModel
{
  private readonly ApplicationDbContext _context;

  [BsonElement("cnes")]
  [DisplayName("CNES")]
  public string? Cnes { get; set; }

  [BsonElement("birth_date")]
  [DisplayName("Data de aniverśario")]
  public DateTime? BirthDate { get; set; }

  [BsonElement("sus_card")]
  [DisplayName("Cartão do SUS")]
  public string? SusCard { get; set; }

  [BsonElement("phone")]
  [DisplayName("Telefone")]
  public string? Phone { get; set; }

  [BsonElement("father_name")]
  [DisplayName("Nome do pai")]
  public string? FatherName { get; set; }

  [BsonElement("mother_name")]
  [DisplayName("Nome da mãe")]
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

public class Professional : UserModel
{
  [BsonElement("academic")]
  [DisplayName("Formação acadêmica")]
  public string? Academic { get; set; }

  [BsonElement("council")]
  [DisplayName("Conselho")]
  public string? Council { get; set; }

  [BsonElement("council_number")]
  [DisplayName("Número do conselho")]
  public string? CouncilNumber { get; set; }

  [BsonElement("specialty")]
  [DisplayName("Especialidade")]
  public string? Specialty { get; set; }

  [BsonElement("formation")]
  [DisplayName("Formação")]
  public string? Formation { get; set; }

  public Professional()
  {
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