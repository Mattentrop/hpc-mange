namespace hpc_mange.Models
{
    public class Software
    {
        public int Id { get; set; }
        public string NomeModulo { get; set; } = string.Empty;
        public string Versao { get; set; } = string.Empty;
        public string Licenca { get; set; } = string.Empty;
        public int? CreatedBy { get; set; }
    }
}