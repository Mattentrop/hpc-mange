namespace hpc_mange.Models // <- O erro CS0234 geralmente é porque falta esse ".Models"
{
    public class Cluster
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public string CapacidadeRede { get; set; }
    }
}