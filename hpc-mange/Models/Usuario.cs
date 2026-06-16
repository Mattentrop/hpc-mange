using System;

namespace hpc_mange.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int? PesquisadorId { get; set; }
    }
}