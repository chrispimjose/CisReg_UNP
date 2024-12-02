using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;
public class Hall
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty; // Necessário para MongoDB
    public string CNPJ { get; set; } = string.Empty;
    public string Cnes { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityHallName { get; set; } = string.Empty;
    public string AgreementNumber { get; set; } = string.Empty;
    public string CityHallManager { get; set; } = string.Empty;
    public string ResponsiblePhoneNumber { get; set; } = string.Empty;
    public string ResponsibleEmail { get; set; } = string.Empty;
}

