using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using hpc_mange.Models;

namespace hpc_mange.DAO
{
    public class ClusterDAO : IDAO<Cluster>
    {
        // Aponta para o IP do seu host Linux (virbr0)
        private readonly string connectionString = "Server=192.168.122.1; Port=3306; Database=hpc_cluster; Uid=admin_hpc; Pwd=suasenha;";

        public List<Cluster> BuscarTodos()
        {
            List<Cluster> lista = new List<Cluster>();

            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Nome, Localizacao, CapacidadeRede FROM Clusters";
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
                    // Mais para frente, o MongoDB vai registrar este erro!
                    Console.WriteLine("Erro ao buscar clusters: " + ex.Message);
                }
            }
            return lista;
        }

        // Os outros métodos exigidos pela interface ficam vazios por enquanto
        public void Inserir(Cluster obj) { throw new NotImplementedException(); }
        public void Atualizar(Cluster obj) { throw new NotImplementedException(); }
        public void Excluir(int id) { throw new NotImplementedException(); }
        public Cluster BuscarPorId(int id) { throw new NotImplementedException(); }
    }
}  