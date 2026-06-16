using System;

namespace hpc_mange.Models
{
    public class Cluster
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string CapacidadeRede { get; set; } = string.Empty;

        public int? PesquisadorId { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string NomePesquisador { get; set; }
    }
}