 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Agente
{
    [Authorize(Roles = "Agente")]
    public class ComisionesModel : PageModel
    {
        private readonly AppDbContext _context;

        public ComisionesModel(AppDbContext context)
        {
            _context = context;
        }

        public decimal ComisionMesActual { get; set; }
        public decimal ComisionMesAnterior { get; set; }
        public List<Multa> UltimasMultasPagadas { get; set; } = new();

        public async Task OnGetAsync()
        {
            var cedulaAgente = User.Identity?.Name;
            
            // Fechas para filtrar
            var inicioMesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var inicioMesAnterior = inicioMesActual.AddMonths(-1);

            // Obtener multas pagadas del mes actual
            var multasMesActual = await _context.Multas
                .Where(m => m.CedulaAgente == cedulaAgente 
                       && m.Estado == "Pagada" 
                       && m.FechaRegistro >= inicioMesActual)
                .SumAsync(m => m.Monto);
            ComisionMesActual = multasMesActual * 0.10m;

            // Obtener multas pagadas del mes anterior
            var multasMesAnterior = await _context.Multas
                .Where(m => m.CedulaAgente == cedulaAgente 
                       && m.Estado == "Pagada" 
                       && m.FechaRegistro >= inicioMesAnterior 
                       && m.FechaRegistro < inicioMesActual)
                .SumAsync(m => m.Monto);
            ComisionMesAnterior = multasMesAnterior * 0.10m;

            // Obtener últimas multas pagadas
            UltimasMultasPagadas = await _context.Multas
                .Include(m => m.Concepto)
                .Where(m => m.CedulaAgente == cedulaAgente && m.Estado == "Pagada")
                .OrderByDescending(m => m.FechaRegistro)
                .Take(5)
                .ToListAsync();
        }
    }
}