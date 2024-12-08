// Pages/Agente/Index.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Agente
{
    [Authorize(Roles = "Agente")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public string NombreAgente { get; set; } = string.Empty;
        public int MultasDelMes { get; set; }
        public int MultasPorCobrar { get; set; }
        public decimal ComisionEstimada { get; set; }

        public async Task OnGetAsync()
        {
            var cedulaAgente = User.Identity?.Name;
            var agente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Cedula == cedulaAgente);

            NombreAgente = agente?.Nombre ?? "No identificado";

            var fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var multasDelAgente = await _context.Multas
                .Where(m => m.CedulaAgente == cedulaAgente)
                .ToListAsync();

            MultasDelMes = multasDelAgente.Count(m => m.FechaRegistro >= fechaInicioMes);
            MultasPorCobrar = multasDelAgente.Count(m => m.Estado == "Activa");
            ComisionEstimada = multasDelAgente
                .Where(m => m.Estado == "Activa")
                .Sum(m => m.Monto) * 0.10m;
        }
    }
}