using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;
using hpc_mange.Interfaces;

namespace hpc_mange.DAO
{
    public class PesquisadorDAO : IDAO<Pesquisador>
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public void Inserir(Pesquisador obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Pesquisadores (Nome, Departamento, Email) VALUES (@nome, @departamento, @email)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@departamento", obj.Departamento);
                comando.Parameters.AddWithValue("@email", obj.Email);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public int InserirRetornandoId(Pesquisador obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Pesquisadores (Nome, Departamento, Email) VALUES (@nome, @departamento, @email); SELECT LAST_INSERT_ID();";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@departamento", obj.Departamento);
                comando.Parameters.AddWithValue("@email", obj.Email);

                conexao.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        public void Atualizar(Pesquisador obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE hpc_cluster.Pesquisadores SET Nome = @nome, Departamento = @departamento, Email = @email WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", obj.Nome);
                comando.Parameters.AddWithValue("@departamento", obj.Departamento);
                comando.Parameters.AddWithValue("@email", obj.Email);
                comando.Parameters.AddWithValue("@id", obj.Id);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE hpc_cluster.Pesquisadores SET deleted_at = CURRENT_TIMESTAMP WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public List<Pesquisador> BuscarTodos()
        {
            List<Pesquisador> lista = new List<Pesquisador>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Pesquisadores WHERE deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                conexao.Open();
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Pesquisador
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                            UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue
                        });
                    }
                }
            }
            return lista;
        }

        public List<Pesquisador> BuscarPorNome(string termo)
        {
            List<Pesquisador> lista = new List<Pesquisador>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Pesquisadores WHERE Nome LIKE @termo AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@termo", "%" + termo + "%");

                conexao.Open();
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Pesquisador
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                            UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue
                        });
                    }
                }
            }
            return lista;
        }

        public Pesquisador BuscarPorId(int id)
        {
            Pesquisador pesquisador = null;
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Pesquisadores WHERE Id = @id AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                conexao.Open();
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pesquisador = new Pesquisador
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Departamento = reader["Departamento"].ToString(),
                            Email = reader["Email"].ToString(),
                            CreatedAt = reader["created_at"] != DBNull.Value ? Convert.ToDateTime(reader["created_at"]) : DateTime.MinValue,
                            UpdatedAt = reader["updated_at"] != DBNull.Value ? Convert.ToDateTime(reader["updated_at"]) : DateTime.MinValue
                        };
                    }
                }
            }
            return pesquisador;
        }
    }
}