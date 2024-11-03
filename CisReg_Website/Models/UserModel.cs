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

[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(Patient), typeof(Professional), typeof(UserHall), typeof(SupUnp), typeof(SupHall), typeof(Admin))]
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
  public Permissions Permission { get; set; }
}

public class Patient : UserModel
{

  public Patient()
  {
    Permission = Permissions.Patient;
  }


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
}

public class Professional : UserModel
{
  public Professional()
  {
    Permission = Permissions.Professional;
  }


  [BsonElement("academic")]
  public string? Academic { get; set; }

  [BsonElement("council")]
  public string? Council { get; set; }

  [BsonElement("council_number")]
  public string? CouncilNumber { get; set; }

  public SpecialtyModel? Specialty { get; set; }
  public FormationModel? Formation { get; set; }
}

public class UserHall : UserModel
{
  public UserHall()
  {
    Permission = Permissions.UserHall;
  }

  [BsonElement("hall")]
  public string? HallModel { get; set; }
}

public class SupUnp : UserModel, IVacancyReserver, IVacancyCreator
{
  public SupUnp()
  {
    Permission = Permissions.SupUnp;
  }
}

public class SupHall : UserHall, IVacancyCreator
{
  public SupHall()
  {
    Permission = Permissions.SupHall;
  }
}

public class Admin : UserModel, IVacancyReserver, IVacancyCreator
{
  public Admin()
  {
    Permission = Permissions.Admin;
  }
}