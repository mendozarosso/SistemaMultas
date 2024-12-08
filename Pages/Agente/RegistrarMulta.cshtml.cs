using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Agente
{
    [Authorize(Roles = "Agente")]
    public class RegistrarMultaModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegistrarMultaModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Multa Multa { get; set; } = new();
        
        public SelectList ConceptosList { get; set; }

        public async Task OnGetAsync()
        {
            // Cargar solo conceptos activos para el dropdown
            var conceptos = await _context.Conceptos
                .Where(c => c.Activo)
                .ToListAsync();

            ConceptosList = new SelectList(conceptos, "Id", "Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Recargar la lista en caso de error
                var conceptos = await _context.Conceptos
                    .Where(c => c.Activo)
                    .ToListAsync();
                ConceptosList = new SelectList(conceptos, "Id", "Nombre");
                return Page();
            }

            var concepto = await _context.Conceptos.FindAsync(Multa.ConceptoId);
            if (concepto == null)
            {
                ModelState.AddModelError("", "El concepto seleccionado no es válido");
                return Page();
            }

            Multa.Monto = concepto.MontoBase;
            Multa.CedulaAgente = User.Identity?.Name ?? "";
            Multa.Estado = "Activa";
            Multa.FechaRegistro = DateTime.Now;

            _context.Multas.Add(Multa);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}