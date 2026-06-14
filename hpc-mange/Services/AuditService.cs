using MongoDB.Driver;
using hpc_mange.Models;
using System;

namespace hpc_mange.Services
{
    public class AuditService
    {
        private readonly IMongoCollection<AuditLog> _logsCollection;

        public AuditService()
        {
            // Usamos o mesmo IP da rede virtual, mas apontando para a porta do Mongo
            string connectionString = "mongodb://admin_mongo:senha_mongo@192.168.122.1:27017";

            var client = new MongoClient(connectionString);

            // Cria (ou acessa) o banco de dados 'hpc_logs'
            var database = client.GetDatabase("hpc_logs");

            // Cria (ou acessa) a coleção 'auditoria'
            _logsCollection = database.GetCollection<AuditLog>("auditoria");
        }

        public void RegistrarLog(string acao, string detalhes)
        {
            try
            {
                var log = new AuditLog
                {
                    Acao = acao,
                    Detalhes = detalhes,
                    DataHora = DateTime.Now
                };

                _logsCollection.InsertOne(log);
            }
            catch (Exception ex)
            {
                // Se o Mongo estiver fora do ar, imprimimos no console para não travar o app
                Console.WriteLine("Erro ao gravar log de auditoria: " + ex.Message);
            }
        }
    }
}