using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class ClusterDAO : IDAO<Cluster>
    {
        // Variável de conexão apontando para o seu IP do Linux (Ajuste a senha se necessário)
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public void Inserir(Cluster obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO Clusters (Nome, Localizacao, CapacidadeRede) VALUES (@nome, @localizacao, @capacidade)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@localizacao", obj.Localizacao);
                comando.Parameters.AddWithValue("@capacidade", obj.CapacidadeRede);

                try
                {
                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao inserir cluster no MySQL: " + ex.Message);
                    throw;
                }
            }
        }

        public List<Cluster> BuscarTodos()
        {
            List<Cluster> lista = new List<Cluster>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Clusters";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                try
                {
                    conexao.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Cluster
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString(),
                                Localizacao = reader["Localizacao"].ToString(),
                                CapacidadeRede = reader["CapacidadeRede"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar no MySQL: " + ex.Message);
                    throw;
                }
            }
            return lista;
        }
        // Os métodos abaixo ficam vazios por enquanto, apenas para satisfazer a interface IDAO
        public void Atualizar(Cluster obj) { throw new NotImplementedException(); }
        public void Excluir(int id) { throw new NotImplementedException(); }
        public Cluster BuscarPorId(int id) { throw new NotImplementedException(); }
        // Os métodos abaixo ficam vazios por enquanto, apenas para satisfazer a interface IDAO
    }
}