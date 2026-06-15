using MySql.Data.MySqlClient;
using System;

namespace hpc_mange.DAO
{
    public class UsuarioDAO
    {
        // Usando a mesma string de conexão do seu ClusterDAO
        private string connectionString = "Server=192.168.122.1;Port=3306;Database=hpc_cluster;Uid=admin_hpc;Pwd=suasenha;";

        public bool ValidarLogin(string login, string senha)
        {
            bool credenciaisValidas = false;

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(1) FROM hpc_cluster.Usuarios WHERE Login = @login AND Senha = @senha";
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
    }
}