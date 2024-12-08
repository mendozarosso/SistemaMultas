 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;

namespace SistemaMultas.Pages.Oficina
{
    [Authorize(Roles = "Oficina")]
    public class ReportesModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportesModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Mes { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Anio { get; set; }

        public decimal TotalIngresos { get; set; }
        public int TotalMultas { get; set; }
        public decimal PromedioMulta { get; set; }
        public List<DetalleConcepto> DetalleConceptos { get; set; } = new();

        public class DetalleConcepto
        {
            public string Concepto { get; set; }
            public int Cantidad { get; set; }
            public decimal Total { get; set; }
            public decimal Promedio => Cantidad > 0 ? Total / Cantidad : 0;
        }

        public async Task OnGetAsync()
        {
            if (Mes == 0) Mes = DateTime.Now.Month;
            if (Anio == 0) Anio = DateTime.Now.Year;

            var fechaInicio = new DateTime(Anio, Mes, 1);
            var fechaFin = fechaInicio.AddMonths(1);

            var multas = await _context.Multas
                .Include(m => m.Concepto)
                .Where(m => m.FechaRegistro >= fechaInicio && 
                           m.FechaRegistro < fechaFin &&
                           m.Estado == "Pagada")
                .ToListAsync();

            TotalMultas = multas.Count;
            TotalIngresos = multas.Sum(m => m.Monto);
            PromedioMulta = TotalMultas > 0 ? TotalIngresos / TotalMultas : 0;

            DetalleConceptos = multas
                .GroupBy(m => m.Concepto.Nombre)
                .Select(g => new DetalleConcepto
                {
                    Concepto = g.Key,
                    Cantidad = g.Count(),
                    Total = g.Sum(m => m.Monto)
                })
                .OrderByDescending(d => d.Total)
                .ToList();
        }
    }
}