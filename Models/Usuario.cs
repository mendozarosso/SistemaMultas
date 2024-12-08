 
using System.ComponentModel.DataAnnotations;

namespace SistemaMultas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}