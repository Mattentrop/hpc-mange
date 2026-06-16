using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class UsuarioDAO
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public bool ValidarLogin(string login, string senha)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM hpc_cluster.Usuarios WHERE Login = @login AND Senha = @senha AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@login", login);
                comando.Parameters.AddWithValue("@senha", senha);

                conexao.Open();
                int resultado = Convert.ToInt32(comando.ExecuteScalar());
                return resultado > 0;
            }
        }

        public void InserirComPesquisador(string login, string senha, int pesquisadorId)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "INSERT INTO hpc_cluster.Usuarios (Login, Senha, PesquisadorId) VALUES (@login, @senha, @pesqId)";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@login", login);
                comando.Parameters.AddWithValue("@senha", senha);
                comando.Parameters.AddWithValue("@pesqId", pesquisadorId);

                conexao.Open();
                comando.ExecuteNonQuery();
            }
        }

        public Usuario BuscarPorEmail(string emailOuLogin)
        {
            Usuario user = null;
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM hpc_cluster.Usuarios WHERE Login = @login AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@login", emailOuLogin);

                conexao.Open();
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Login = reader["Login"].ToString(),
                            PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : null
                        };
                    }
                }
            }
            return user;
        }
    }
}