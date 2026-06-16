using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace hpc_mange.Models 
{
    public class LogAuditoria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Operacao { get; set; }
        public DateTime DataHora { get; set; }
        public string Detalhes { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}