using MongoDB.Driver;
using hpc_mange.Models;
using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace hpc_mange.Services
{
    public class AuditService
    {
        public static void RegistrarLog(string operacao, string detalhes, string usuario)
        {
            try
            {
                string connectionString = "mongodb://admin_mongo:senha_mongo@192.168.122.1:27017/?authSource=admin";
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase("hpc_audit");
                var collection = database.GetCollection<LogAuditoria>("logs");

                var log = new LogAuditoria
                {
                    Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                    Operacao = operacao,
                    Detalhes = detalhes,
                    DataHora = DateTime.Now,
                    UsuarioResponsavel = string.IsNullOrWhiteSpace(usuario) ? "Sistema" : usuario
                };

                collection.InsertOne(log);
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Falha Oculta no MongoDB", ex.Message, "OK");
                    }
                });
            }
        }
    }
}