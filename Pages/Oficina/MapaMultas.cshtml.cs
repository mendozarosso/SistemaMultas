using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Oficina
{
    [Authorize(Roles = "Oficina")]
    public class MapaMultasModel : PageModel
    {
        private readonly AppDbContext _context;

        public MapaMultasModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Mes { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Anio { get; set; }

        public List<Multa> Multas { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (Mes == 0) Mes = DateTime.Now.Month;
            if (Anio == 0) Anio = DateTime.Now.Year;

            var fechaInicio = new DateTime(Anio, Mes, 1);
            var fechaFin = fechaInicio.AddMonths(1);

            Multas = await _context.Multas
                .Include(m => m.Concepto)
                .Where(m => m.FechaRegistro >= fechaInicio && 
                           m.FechaRegistro < fechaFin)
                .ToListAsync();
        }
    }
}