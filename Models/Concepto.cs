 
using System.ComponentModel.DataAnnotations;

namespace SistemaMultas.Models
{
    public class Concepto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal MontoBase { get; set; }
        public bool Activo { get; set; } = true;
    }
}
