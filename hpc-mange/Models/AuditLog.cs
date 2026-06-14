using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace hpc_mange.Models
{
    public class AuditLog
    {
        // O BsonId diz para o MongoDB que essa é a chave primária gerada automaticamente
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Acao { get; set; }
        public string Detalhes { get; set; }
        public DateTime DataHora { get; set; }
    }
}