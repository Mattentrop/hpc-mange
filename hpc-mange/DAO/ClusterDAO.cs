using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class ClusterDAO : IDAO<Cluster>
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public void Inserir(Cluster obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Clusters (Nome, Localizacao, CapacidadeRede, PesquisadorId, created_by) VALUES (@nome, @localizacao, @capacidade, @pesquisadorId, @createdBy)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@localizacao", obj.Localizacao);
                comando.Parameters.AddWithValue("@capacidade", obj.CapacidadeRede);
                comando.Parameters.AddWithValue("@pesquisadorId", obj.PesquisadorId.HasValue ? (object)obj.PesquisadorId.Value : DBNull.Value);
                comando.Parameters.AddWithValue("@createdBy", obj.CreatedBy.HasValue ? (object)obj.CreatedBy.Value : DBNull.Value);

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
                string sql = @"SELECT c.*, p.Nome AS NomePesquisador 
                               FROM hpc_cluster.Clusters c 
                               LEFT JOIN hpc_cluster.Pesquisadores p ON c.PesquisadorId = p.Id 
                               WHERE c.deleted_at IS NULL";
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
                                CapacidadeRede = reader["CapacidadeRede"].ToString(),
                                PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : null,
                                CreatedBy = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : null,
                                CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                                UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue,
                                NomePesquisador = reader["NomePesquisador"] != DBNull.Value ? reader["NomePesquisador"].ToString() : "Não Atribuído"
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
                string sql = "UPDATE hpc_cluster.Clusters SET Nome = @nome, Localizacao = @loc, CapacidadeRede = @cap, PesquisadorId = @pesquisadorId WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@loc", obj.Localizacao);
                comando.Parameters.AddWithValue("@cap", obj.CapacidadeRede);
                comando.Parameters.AddWithValue("@pesquisadorId", obj.PesquisadorId.HasValue ? (object)obj.PesquisadorId.Value : DBNull.Value);
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
                try
                {
                    conexao.Open();
                    using (MySqlTransaction transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            string sqlNodos = "UPDATE hpc_cluster.Nodos SET deleted_at = CURRENT_TIMESTAMP WHERE ClusterId = @id AND deleted_at IS NULL";
                            using (MySqlCommand cmdNodos = new MySqlCommand(sqlNodos, conexao, transacao))
                            {
                                cmdNodos.Parameters.AddWithValue("@id", id);
                                cmdNodos.ExecuteNonQuery();
                            }

                            string sqlCluster = "UPDATE hpc_cluster.Clusters SET deleted_at = CURRENT_TIMESTAMP WHERE Id = @id";
                            using (MySqlCommand cmdCluster = new MySqlCommand(sqlCluster, conexao, transacao))
                            {
                                cmdCluster.Parameters.AddWithValue("@id", id);
                                cmdCluster.ExecuteNonQuery();
                            }

                            transacao.Commit();
                        }
                        catch (Exception)
                        {
                            transacao.Rollback();
                            throw;
                        }
                    }
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
                string sql = @"SELECT c.*, p.Nome AS NomePesquisador 
                               FROM hpc_cluster.Clusters c 
                               LEFT JOIN hpc_cluster.Pesquisadores p ON c.PesquisadorId = p.Id 
                               WHERE c.Nome LIKE @termo AND c.deleted_at IS NULL";
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
                                CapacidadeRede = reader["CapacidadeRede"].ToString(),
                                PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : null,
                                CreatedBy = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : null,
                                CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                                UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue,
                                NomePesquisador = reader["NomePesquisador"] != DBNull.Value ? reader["NomePesquisador"].ToString() : "Não Atribuído"
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

        public List<Cluster> BuscarComFiltro(string termoNome, string capacidade)
        {
            List<Cluster> lista = new List<Cluster>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.*, p.Nome AS NomePesquisador 
                               FROM hpc_cluster.Clusters c 
                               LEFT JOIN hpc_cluster.Pesquisadores p ON c.PesquisadorId = p.Id 
                               WHERE c.deleted_at IS NULL";

                if (!string.IsNullOrWhiteSpace(termoNome))
                    sql += " AND c.Nome LIKE @termo";

                if (!string.IsNullOrWhiteSpace(capacidade))
                    sql += " AND c.CapacidadeRede = @capacidade";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                if (!string.IsNullOrWhiteSpace(termoNome))
                    comando.Parameters.AddWithValue("@termo", "%" + termoNome + "%");

                if (!string.IsNullOrWhiteSpace(capacidade))
                    comando.Parameters.AddWithValue("@capacidade", capacidade);

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
                                CapacidadeRede = reader["CapacidadeRede"].ToString(),
                                PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : null,
                                CreatedBy = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : null,
                                CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                                UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue,
                                NomePesquisador = reader["NomePesquisador"] != DBNull.Value ? reader["NomePesquisador"].ToString() : "Não Atribuído"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar com filtro no MySQL: " + ex.Message);
                    throw;
                }
            }
            return lista;
        }

        public List<string> BuscarCapacidadesDistintas()
        {
            List<string> lista = new List<string>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT DISTINCT CapacidadeRede FROM hpc_cluster.Clusters WHERE deleted_at IS NULL ORDER BY CapacidadeRede";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                try
                {
                    conexao.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(reader["CapacidadeRede"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar capacidades distintas no MySQL: " + ex.Message);
                    throw;
                }
            }
            return lista;
        }

        public Cluster BuscarPorId(int id)
        {
            Cluster cluster = null;
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.*, p.Nome AS NomePesquisador 
                               FROM hpc_cluster.Clusters c 
                               LEFT JOIN hpc_cluster.Pesquisadores p ON c.PesquisadorId = p.Id 
                               WHERE c.Id = @id AND c.deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                try
                {
                    conexao.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cluster = new Cluster
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nome = reader["Nome"].ToString(),
                                Localizacao = reader["Localizacao"].ToString(),
                                CapacidadeRede = reader["CapacidadeRede"].ToString(),
                                PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : null,
                                CreatedBy = reader["created_by"] != DBNull.Value ? Convert.ToInt32(reader["created_by"]) : null,
                                CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                                UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue,
                                NomePesquisador = reader["NomePesquisador"] != DBNull.Value ? reader["NomePesquisador"].ToString() : "Não Atribuído"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar por ID no MySQL: " + ex.Message);
                    throw;
                }
            }
            return cluster;
        }
    }
}