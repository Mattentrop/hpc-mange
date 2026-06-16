using MySql.Data.MySqlClient;
using System;

namespace hpc_mange.DAO
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public int? PesquisadorId { get; set; }
    }

    public class UsuarioDAO
    {
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public bool ValidarLogin(string login, string senha)
        {
            bool credenciaisValidas = false;

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(1) FROM hpc_cluster.Usuarios WHERE Login = @login AND Senha = @senha AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@login", login);
                comando.Parameters.AddWithValue("@senha", senha);

                try
                {
                    conexao.Open();
                    int count = Convert.ToInt32(comando.ExecuteScalar());
                    if (count > 0)
                    {
                        credenciaisValidas = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao validar login no MySQL: " + ex.Message);
                }
            }

            return credenciaisValidas;
        }

        public Usuario BuscarPorEmail(string email)
        {
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Login, PesquisadorId FROM hpc_cluster.Usuarios WHERE Login = @email AND deleted_at IS NULL";
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@email", email);

                try
                {
                    conexao.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Login = reader["Login"].ToString(),
                                PesquisadorId = reader["PesquisadorId"] != DBNull.Value ? Convert.ToInt32(reader["PesquisadorId"]) : (int?)null
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar usuário por e-mail: " + ex.Message);
                }
            }
            return null;
        }
    }
}