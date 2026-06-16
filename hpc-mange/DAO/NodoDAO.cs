using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class NodoDAO
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public List<Nodo> BuscarPorCluster(int clusterId)
        {
            List<Nodo> lista = new List<Nodo>();
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Nodos WHERE ClusterId = @clusterId AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@clusterId", clusterId);

                conexao.Open();
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Nodo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Hostname = reader["Hostname"]?.ToString() ?? "",
                            MemoriaRAM = reader["MemoriaRAM"] != DBNull.Value ? Convert.ToInt32(reader["MemoriaRAM"]) : 0,
                            ModeloCPU = reader["ModeloCPU"]?.ToString() ?? "",
                            ClusterId = Convert.ToInt32(reader["ClusterId"])
                        });
                    }
                }
            }
            return lista;
        }

        public void Inserir(Nodo nodo)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Nodos (Hostname, MemoriaRAM, ModeloCPU, ClusterId) VALUES (@host, @ram, @cpu, @clusterId)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@host", nodo.Hostname);
                comando.Parameters.AddWithValue("@ram", nodo.MemoriaRAM);
                comando.Parameters.AddWithValue("@cpu", nodo.ModeloCPU);
                comando.Parameters.AddWithValue("@clusterId", nodo.ClusterId);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public int InserirRetornandoId(Nodo nodo)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Nodos (Hostname, MemoriaRAM, ModeloCPU, ClusterId) VALUES (@host, @ram, @cpu, @clusterId); SELECT LAST_INSERT_ID();";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@host", nodo.Hostname);
                comando.Parameters.AddWithValue("@ram", nodo.MemoriaRAM);
                comando.Parameters.AddWithValue("@cpu", nodo.ModeloCPU);
                comando.Parameters.AddWithValue("@clusterId", nodo.ClusterId);

                conexao.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        public void Excluir(int id)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE hpc_cluster.Nodos SET deleted_at = CURRENT_TIMESTAMP WHERE Id = @id";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@id", id);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void VincularSoftware(int nodoId, int softwareId)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT IGNORE INTO hpc_cluster.Nodos_Softwares (NodoId, SoftwareId, DataInstalacao) VALUES (@nodoId, @softwareId, CURRENT_DATE)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@nodoId", nodoId);
                comando.Parameters.AddWithValue("@softwareId", softwareId);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}