using SQLite;
using hpc_mange.Models;
using System;
using System.IO;

namespace hpc_mange.Services
{
    public class SQLiteService
    {
        private SQLiteConnection _db;

        public SQLiteService()
        {
            // Pega o caminho oficial de dados do aplicativo (funciona no Windows, Android, iOS, etc.)
            string pastaApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string caminhoBanco = Path.Combine(pastaApp, "hpc_offline.db3");

            // Abre a conexão (se o arquivo não existir, o SQLite cria na hora)
            _db = new SQLiteConnection(caminhoBanco);

            // Cria a tabela se for a primeira vez rodando
            _db.CreateTable<ConfiguracaoLocal>();
        }

        public void SalvarConfiguracao(string chave, string valor)
        {
            // Verifica se a chave já existe para atualizar, ou insere uma nova
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