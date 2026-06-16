using System;

namespace hpc_mange.Models
{
    public class Pesquisador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}