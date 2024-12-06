using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CisReg_Website.Models
{
    public class ScheduleModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("professionalId")]
        public ObjectId? professionalId { get; set; }

        [BsonElement("Time")]
        [DisplayName("Horario")]
        public string? Time { get; set; }

        [BsonElement("work_days")]
        [DisplayName("Dias de trabalho")]
        [NotMapped]
        public Dictionary<Date, bool> WorkDays { get; set; } = new Dictionary<Date, bool>();

        [BsonElement("status_pacient")]
        [DisplayName("Status do Paciente")]//aqui mostra se está reservado, cancelado a consulta ou disponivel a vaga
        public string? StatusPacient { get; set; }

    }
}
