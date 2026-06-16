using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class SoftwareDAO
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public List<Software> BuscarTodos()
        {
            List<Software> lista = new List<Software>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Softwares WHERE deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                conexao.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Software
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NomeModulo = reader["NomeModulo"]?.ToString() ?? "",
                            Versao = reader["Versao"]?.ToString() ?? "",
                            Licenca = reader["Licenca"]?.ToString() ?? ""
                        });
                    }
                }
            }
            return lista;
        }

        public List<Software> BuscarPorNodo(int nodoId)
        {
            List<Software> lista = new List<Software>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT s.* FROM hpc_cluster.Softwares s
                               INNER JOIN hpc_cluster.Nodos_Softwares ns ON s.Id = ns.SoftwareId
                               WHERE ns.NodoId = @nodoId AND s.deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nodoId", nodoId);

                conexao.Open();
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Software
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NomeModulo = reader["NomeModulo"].ToString(),
                            Versao = reader["Versao"].ToString(),
                            Licenca = reader["Licenca"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public void Inserir(Software obj)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Softwares (NomeModulo, Versao, Licenca) VALUES (@nome, @versao, @licenca)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nome", obj.NomeModulo);
                comando.Parameters.AddWithValue("@versao", obj.Versao);
                comando.Parameters.AddWithValue("@licenca", obj.Licenca);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE hpc_cluster.Softwares SET deleted_at = CURRENT_TIMESTAMP WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}