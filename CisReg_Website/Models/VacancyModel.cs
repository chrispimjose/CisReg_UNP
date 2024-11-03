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
  public Patient? Patient { get; set; }

  [BsonElement("professional")]
  public Professional? Professional { get; set; }

  [BsonElement("reserved_by")]
  public IVacancyReserver? ReservedBy { get; set; }

  [BsonElement("created_by")]
  public IVacancyCreator? CreatedBy { get; set; }

  [BsonElement("status")]
  [BsonRepresentation(BsonType.String)]
  public Status Status { get; set; }
}