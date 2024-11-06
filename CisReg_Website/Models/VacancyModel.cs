using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;

public enum Status
{
  Available,
  Occupied,
  Canceled,
  Rescheduled,
  Vazio
}
public class VacancyModel
{
  [BsonId]
  public ObjectId Id { get; set; }

    [BsonElement("available_hour")]
    public List<DateTime> AvailableHour { get; set; }  // Mantido como DateTime, assumindo um único horário



    [BsonElement("patient")]
  public Patient? Patient { get; set; }

  [BsonElement("professional")]
  public Professional? Professional { get; set; }

    [BsonElement("reserved_by")]
    public List<UserModel>? ReservedBy { get; set; }  // Alterado para uma lista de UserModel
                                                      // Mudei para UserModel ou uma implementação concreta

    [BsonElement("created_by")]
    public UserHall? CreatedBy { get; set; }  // Mudei para UserModel ou uma implementação concreta


    [BsonElement("status")]
  [BsonRepresentation(BsonType.String)]
  public Status Status { get; set; }
}