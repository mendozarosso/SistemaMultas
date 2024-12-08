 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Agente
{
    [Authorize(Roles = "Agente")]
    public class ListadoMultasModel : PageModel
    {
        private readonly AppDbContext _context;

        public ListadoMultasModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Multa> Multas { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string FiltroEstado { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Multas
                .Include(m => m.Concepto)
                .Where(m => m.CedulaAgente == User.Identity.Name)
                .OrderByDescending(m => m.FechaRegistro);

            if (!string.IsNullOrEmpty(FiltroEstado))
            {
                query = (IOrderedQueryable<Multa>)query.Where(m => m.Estado == FiltroEstado);
            }

            Multas = await query.ToListAsync();
        }
    }
}