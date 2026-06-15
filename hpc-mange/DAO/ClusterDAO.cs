using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class ClusterDAO : IDAO<Cluster>
    {
        // Variável de conexão apontando para o seu IP do Linux
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public void Inserir(Cluster obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Clusters (Nome, Localizacao, CapacidadeRede) VALUES (@nome, @localizacao, @capacidade)";
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
                string sql = "SELECT * FROM hpc_cluster.Clusters";
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

        public void Atualizar(Cluster obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE hpc_cluster.Clusters SET Nome = @nome, Localizacao = @loc, CapacidadeRede = @cap WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@loc", obj.Localizacao);
                comando.Parameters.AddWithValue("@cap", obj.CapacidadeRede);
                comando.Parameters.AddWithValue("@id", obj.Id);

                try
                {
                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar cluster no MySQL: " + ex.Message);
                    throw;
                }
            }
        }

        public void Excluir(int id)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "DELETE FROM hpc_cluster.Clusters WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                try
                {
                    conexao.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao excluir cluster no MySQL: " + ex.Message);
                    throw;
                }
            }
        }

        public List<Cluster> BuscarPorNome(string termo)
        {
            List<Cluster> lista = new List<Cluster>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                // O LIKE com % permite achar partes da palavra
                string sql = "SELECT * FROM hpc_cluster.Clusters WHERE Nome LIKE @termo";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@termo", "%" + termo + "%");

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
                    Console.WriteLine("Erro ao buscar por nome no MySQL: " + ex.Message);
                    throw;
                }
            }
            return lista;
        }

        // O método BuscarPorId continua vazio apenas para satisfazer a interface IDAO (se ela exigir)
        public Cluster BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}