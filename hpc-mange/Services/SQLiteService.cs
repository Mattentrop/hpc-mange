using SQLite;
using System;
using System.IO;

namespace hpc_mange.Services
{
    // Modelo da tabela local no dispositivo
    public class ConfiguracaoLocal
    {
        [PrimaryKey]
        public string Chave { get; set; }
        public string Valor { get; set; }
    }

    public class SQLiteService
    {
        private SQLiteConnection _db;

        public SQLiteService()
        {
            string pastaApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string caminhoBanco = Path.Combine(pastaApp, "hpc_offline.db3");

            _db = new SQLiteConnection(caminhoBanco);
            _db.CreateTable<ConfiguracaoLocal>();
        }

        public void SalvarConfiguracao(string chave, string valor)
        {
            var configExistente = _db.Table<ConfiguracaoLocal>().FirstOrDefault(c => c.Chave == chave);

            if (configExistente == null)
            {
                _db.Insert(new ConfiguracaoLocal { Chave = chave, Valor = valor });
            }
            else
            {
                configExistente.Valor = valor;
                _db.Update(configExistente);
            }
        }

        public string LerConfiguracao(string chave)
        {
            var config = _db.Table<ConfiguracaoLocal>().FirstOrDefault(c => c.Chave == chave);
            return config != null ? config.Valor : "Nenhuma configuração encontrada";
        }
    }
}