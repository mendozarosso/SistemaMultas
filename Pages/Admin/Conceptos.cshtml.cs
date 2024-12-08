 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ConceptosModel : PageModel
    {
        private readonly AppDbContext _context;

        public ConceptosModel(AppDbContext context)
        {
            _context = context;
        }

        public class ConceptoViewModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal MontoBase { get; set; }
            public bool Activo { get; set; }
            public int TotalMultas { get; set; }
        }

        public List<ConceptoViewModel> Conceptos { get; set; } = new();

        public async Task OnGetAsync()
        {
            Conceptos = await _context.Conceptos
                .Select(c => new ConceptoViewModel
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    MontoBase = c.MontoBase,
                    Activo = c.Activo,
                    TotalMultas = _context.Multas.Count(m => m.ConceptoId == c.Id)
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(Concepto concepto)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (concepto.Id == 0)
            {
                concepto.Activo = true;
                _context.Conceptos.Add(concepto);
            }
            else
            {
                var conceptoExistente = await _context.Conceptos.FindAsync(concepto.Id);
                if (conceptoExistente != null)
                {
                    conceptoExistente.Nombre = concepto.Nombre;
                    conceptoExistente.MontoBase = concepto.MontoBase;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(int id)
        {
            var concepto = await _context.Conceptos.FindAsync(id);
            if (concepto != null)
            {
                concepto.Activo = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivarAsync(int id)
        {
            var concepto = await _context.Conceptos.FindAsync(id);
            if (concepto != null)
            {
                concepto.Activo = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}