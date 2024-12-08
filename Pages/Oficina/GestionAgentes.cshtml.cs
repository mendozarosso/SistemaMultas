using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Oficina
{
    [Authorize(Roles = "Oficina")]
    public class GestionAgentesModel : PageModel
    {
        private readonly AppDbContext _context;

        public GestionAgentesModel(AppDbContext context)
        {
            _context = context;
        }

        public class AgenteViewModel
        {
            public int Id { get; set; }
            public string Cedula { get; set; }
            public string Nombre { get; set; }
            public bool Activo { get; set; }
            public int TotalMultas { get; set; }
        }

        public List<AgenteViewModel> Agentes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Agentes = await _context.Usuarios
                .Where(u => u.Tipo == "Agente")
                .Select(u => new AgenteViewModel
                {
                    Id = u.Id,
                    Cedula = u.Cedula,
                    Nombre = u.Nombre,
                    Activo = u.Activo,
                    TotalMultas = _context.Multas.Count(m => m.CedulaAgente == u.Cedula)
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            usuario.Tipo = "Agente";
            usuario.Activo = true;
            usuario.Clave = BCrypt.Net.BCrypt.HashPassword(usuario.Clave);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(int id)
        {
            var agente = await _context.Usuarios.FindAsync(id);
            if (agente != null)
            {
                agente.Activo = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivarAsync(int id)
        {
            var agente = await _context.Usuarios.FindAsync(id);
            if (agente != null)
            {
                agente.Activo = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}