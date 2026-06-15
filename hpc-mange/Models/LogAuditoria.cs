namespace hpc_mange.Models
{
    public class LogAuditoria
    {
        public object Id { get; set; }
        public string Operacao { get; set; }
        public DateTime DataHora { get; set; }
        public string Detalhes { get; set; }
    }
}