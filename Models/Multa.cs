using System.ComponentModel.DataAnnotations;

namespace SistemaMultas.Models
{
    public class Multa
    {
        public int Id { get; set; }
        public string CedulaCiudadano { get; set; } = string.Empty;
        public string NombreCiudadano { get; set; } = string.Empty;
        public int ConceptoId { get; set; }
        public virtual Concepto Concepto { get; set; }  
        public string Descripcion { get; set; } = string.Empty;
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? FotoUrl { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Activa";
        public string CedulaAgente { get; set; } = string.Empty;
        public virtual Usuario Agente { get; set; }     
        public decimal Monto { get; set; }
    }
}