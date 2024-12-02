using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;

public class Address
{
  public string? City { get; set; }
  public string? Zipcode { get; set; }
  public string? StateName { get; set; }
  public string? StreetName { get; set; }
  public string? Number { get; set; }
  public string? Neighborhood { get; set; }
}

public class HallModel
{
  [BsonId]
  public ObjectId Id { get; set; }

  [BsonElement("cnpj")]
  public string? CNPJ { get; set; }

  [BsonElement("cnes")]
  public int CNES { get; set; }

  [BsonElement("agreement")]
  public int Agreement { get; set; }

  [BsonElement("address")]
  public Address? Address { get; set; }

  [BsonElement]("")

public string CityHallName { get; set; } = string.Empty;
    public string AgreementNumber { get; set; } = string.Empty;
    public string CityHallManager { get; set; } = string.Empty;
    public string ResponsiblePhoneNumber { get; set; } = string.Empty;
    public string ResponsibleEmail { get; set; } = string.Empty;

}