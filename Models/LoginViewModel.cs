 
using System.ComponentModel.DataAnnotations;

namespace SistemaMultas.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "La cédula es requerida")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es requerida")]
        [Display(Name = "Clave")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;
    }
}