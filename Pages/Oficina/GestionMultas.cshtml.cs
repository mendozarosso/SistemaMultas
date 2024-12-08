 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Oficina
{
    [Authorize(Roles = "Oficina")]
    public class GestionMultasModel : PageModel
    {
        private readonly AppDbContext _context;

        public GestionMultasModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Multa> Multas { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Busqueda { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Estado { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Multas
                .Include(m => m.Concepto)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Busqueda))
            {
                query = query.Where(m => 
                    m.CedulaCiudadano.Contains(Busqueda) || 
                    m.NombreCiudadano.Contains(Busqueda));
            }

            if (!string.IsNullOrEmpty(Estado))
            {
                query = query.Where(m => m.Estado == Estado);
            }

            Multas = await query
                .OrderByDescending(m => m.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostPagarAsync(int multaId)
        {
            var multa = await _context.Multas.FindAsync(multaId);
            if (multa != null && multa.Estado == "Activa")
            {
                multa.Estado = "Pagada";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { Busqueda, Estado });
        }

        public async Task<IActionResult> OnPostPerdonarAsync(int multaId)
        {
            var multa = await _context.Multas.FindAsync(multaId);
            if (multa != null && multa.Estado == "Activa")
            {
                multa.Estado = "Perdonada";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { Busqueda, Estado });
        }
    }
}