using CisReg_Website.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisReg_Website.Models;

public class UserModel : DataFoundation
{
    [BsonRepresentation(BsonType.String)]
    public string? Name { get; set; }

    [BsonRepresentation(BsonType.String)]
    public string? Email { get; set; }
}