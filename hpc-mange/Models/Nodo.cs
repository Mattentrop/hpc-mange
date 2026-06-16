using System;
using System.Collections.Generic;

namespace hpc_mange.Models
{
    public class Nodo
    {
        public int Id { get; set; }
        public string Hostname { get; set; } = string.Empty;
        public int MemoriaRAM { get; set; }
        public string ModeloCPU { get; set; } = string.Empty;
        public int ClusterId { get; set; }
        public int? CreatedBy { get; set; }

        public List<Software> SoftwaresInstalados { get; set; } = new List<Software>();
    }
}