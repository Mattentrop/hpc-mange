using SQLite;

namespace hpc_mange.Models
{
    public class ConfiguracaoLocal
    {
        // O SQLite usa atributos próprios para gerenciar o banco
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Chave { get; set; }

        public string Valor { get; set; }
    }
}