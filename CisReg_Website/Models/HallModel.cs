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

  [BsonElement("cityHallName")]
  public string CityHallName { get; set; } = string.Empty;

  [BsonElement("agreementNumber")]
  public string AgreementNumber { get; set; } = string.Empty;

  [BsonElement("cityHallManager")]
  public string CityHallManager { get; set; } = string.Empty;

  [BsonElement("responsiblePhoneNumber")]
  public string ResponsiblePhoneNumber { get; set; } = string.Empty;

  [BsonElement("responsibleEmail")]
  public string ResponsibleEmail { get; set; } = string.Empty;

// Optional constructor to initialize required fields
    public HallModel()
    {
        CityHallName = string.Empty;
        AgreementNumber = string.Empty;
        CityHallManager = string.Empty;
        ResponsiblePhoneNumber = string.Empty;
        ResponsibleEmail = string.Empty;
    }
}