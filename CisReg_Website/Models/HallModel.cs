using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

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
    [BsonId] // Marca a propriedade como o identificador único do MongoDB
    public ObjectId Id { get; set; }
    [BsonElement("cnpj")]
  public string? CNPJ { get; set; }

  [BsonElement("cnes")]
  public int CNES { get; set; }

  [BsonElement("agreement")]
  public int Agreement { get; set; }

  [BsonElement("address")]
  public Address? Address { get; set; }
}
