using System.ComponentModel;
using CisReg_Website.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    [BsonElement("cpf")]
    [DisplayName("CPF")]
    public string? CPF { get; set; }

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

    public Patient()
    {

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

    [BsonElement("specialty")]
    [DisplayName("Especialidade")]
    public string? Specialty { get; set; }

    public Professional()
    {

    }
}

public class UserHall : UserModel
{
    [BsonElement("hall")]
    [DisplayName("Prefeitura")]
    public string? HallModel { get; set; }

    [BsonElement("phone")]
    [DisplayName("Telefone")]
    public string? Phone { get; set; }
}

public class SupUnp : UserModel
{

    [BsonElement("phone")]
    [DisplayName("Telefone")]
    public string? Phone { get; set; }

    [BsonElement("address")]
    [DisplayName("Endereço")]
    public string? Address { get; set; }

    [BsonElement("birth_date")]
    [DisplayName("Data de Nascimento")]
    public Date? BirthDate { get; set; }

    [BsonElement("position")]
    [DisplayName("Cargo")]
    public string? Position { get; set; }

    [BsonElement("department")]
    [DisplayName("Departamento")]
    public string? Department { get; set; }

    [BsonElement("workShift")]
    [DisplayName("Turno de Trabalho")]
    public string? WorkShift { get; set; }
}

public class Admin : UserModel
{

}