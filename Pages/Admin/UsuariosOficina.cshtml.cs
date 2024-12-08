using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SistemaMultas.Data;
using SistemaMultas.Models;

namespace SistemaMultas.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsuariosOficinaModel : PageModel
    {
        private readonly AppDbContext _context;

        public UsuariosOficinaModel(AppDbContext context)
        {
            _context = context;
        }

        public class UsuarioViewModel
        {
            public int Id { get; set; }
            public string Cedula { get; set; }
            public string Nombre { get; set; }
            public bool Activo { get; set; }
            public DateTime? UltimoAcceso { get; set; }
        }

        public List<UsuarioViewModel> Usuarios { get; set; } = new();
        [TempData]
        public string Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            Usuarios = await _context.Usuarios
                .Where(u => u.Tipo == "Oficina")
                .Select(u => new UsuarioViewModel
                {
                    Id = u.Id,
                    Cedula = u.Cedula,
                    Nombre = u.Nombre,
                    Activo = u.Activo,
                    UltimoAcceso = null // Puedes implementar el tracking de último acceso si lo deseas
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (usuario.Id == 0)
            {
                if (string.IsNullOrEmpty(usuario.Clave))
                {
                    ModelState.AddModelError("Clave", "La clave es requerida para nuevos usuarios");
                    return Page();
                }

                usuario.Tipo = "Oficina";
                usuario.Activo = true;
                usuario.Clave = BCrypt.Net.BCrypt.HashPassword(usuario.Clave);
                _context.Usuarios.Add(usuario);
                Mensaje = "Usuario creado exitosamente";
            }
            else
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
                if (usuarioExistente != null)
                {
                    usuarioExistente.Cedula = usuario.Cedula;
                    usuarioExistente.Nombre = usuario.Nombre;
                    if (!string.IsNullOrEmpty(usuario.Clave))
                    {
                        usuarioExistente.Clave = BCrypt.Net.BCrypt.HashPassword(usuario.Clave);
                    }
                    Mensaje = "Usuario actualizado exitosamente";
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDesactivarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Activo = false;
                await _context.SaveChangesAsync();
                Mensaje = "Usuario desactivado exitosamente";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Activo = true;
                await _context.SaveChangesAsync();
                Mensaje = "Usuario activado exitosamente";
            }
            return RedirectToPage();
        }
    }
}