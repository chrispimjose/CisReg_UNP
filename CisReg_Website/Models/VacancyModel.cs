using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;

public enum Status
{
  Available,
  Occupied,
  Canceled,
  Rescheduled
}
public class VacancyModel
{
  [BsonId]
  public ObjectId Id { get; set; }

  [BsonElement("available_hour")]
  public DateTime AvailableHour { get; set; }

  [BsonElement("patient")]
  public ObjectId? PatientId { get; set; }

  [BsonElement("professional")]
  public ObjectId? ProfessionalId { get; set; }

  [BsonElement("reserved_by")]
  public ObjectId? ReservedById { get; set; }

  [BsonElement("created_by")]
  public ObjectId? CreatedById { get; set; }

  [BsonElement("status")]
  [BsonRepresentation(BsonType.String)]
  public Status Status { get; set; }
}